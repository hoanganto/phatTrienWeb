using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StationShop.AppData;
using StationShop.Models;

namespace StationShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _db;
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        public ProductController(AppDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            Product? p = _db.Products.Find(id);
            return View(p);
        }

        public IActionResult ListPro(int id)
        {
            List<Product> prolist = _db.Products.Where(p => p.CategoryId == id).ToList();
            return View(prolist);
        }

        /// Thêm sản phẩm vào cart
        [Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int productid)
        {
            var product = _db.Products.Where(p => p.Id == productid).FirstOrDefault();
            if (product == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, product = product });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }

        /// xóa item trong cart
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromBody] List<CartUpdateModel> cartUpdates)
        {
            var cart = GetCartItems();

            foreach (var update in cartUpdates)
            {
                var cartitem = cart.Find(p => p.product.Id == update.productid);
                if (cartitem != null)
                {
                    cartitem.quantity = update.quantity;
                }
            }

            SaveCartSession(cart);
            return Ok();
        }

        public class CartUpdateModel
        {
            public int productid { get; set; }
            public int quantity { get; set; }
        }

        // Hiện thị giỏ hàng
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }

        [Route("/checkout", Name = "checkout")]
        public IActionResult CheckOut()
        {
            // Xử lý khi đặt hàng
            return View();
        }

        List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY) ?? string.Empty;
            if (jsoncart != string.Empty)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart) ?? new List<CartItem>();
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        public IActionResult PlaceOrder()
        {
            var cartItems = GetCartItems();
            if (cartItems.Count <= 0)
            {
                ViewBag.IsPurchaseSuccessful = false;
                return View();
            }

            Order order = new Order()
            {
                CreatedAt = DateTime.Now,
                Status = "Ordered",
                UserId = User.Identity?.Name ?? string.Empty
            };
            _db.Orders.Add(order);
            _db.SaveChanges();
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            foreach (var product in cartItems)
            {
                orderProducts.Add(new OrderProduct()
                {
                    OrderId = order.Id ?? 0,
                    ProductId = product.product.Id,
                    Quantity = product.quantity,
                    Price = product.product.Price
                });
            }
            // Add the item to the database

            _db.OrderProducts.AddRange(orderProducts);
            _db.SaveChanges(); // Save changes to persist the new item.
            ViewBag.IsPurchaseSuccessful = true;
            ClearCart();
            return View(); // Redirect back to the Index page.
        }
    }
}
