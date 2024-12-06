using Microsoft.AspNetCore.Mvc;
using StationShop.AppData;
using StationShop.Models;

namespace StationShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _db;

        public ProductController( AppDBContext db)
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
    }
}
