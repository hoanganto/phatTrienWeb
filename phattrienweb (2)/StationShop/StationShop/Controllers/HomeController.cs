using Microsoft.AspNetCore.Mvc;
using StationShop.AppData;
using StationShop.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;


namespace StationShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _db;

        public HomeController(ILogger<HomeController> logger, AppDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {

            List<Product> proList = _db.Products.Take(4).ToList();
            return View(proList);

        }

        public IActionResult Privacy()
        {
            List<Category> categoies = _db.Categories.ToList();
            return View(categoies);
        }       
    }
}
