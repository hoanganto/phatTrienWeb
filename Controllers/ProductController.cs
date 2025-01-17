﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StationShop.AppData;
using StationShop.Models;

namespace StationShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _db;
        

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

        ///http://localhost:5138/Product/ListPro/2
        public IActionResult ListPro(int id)
        {
            List<Product> prolist = _db.Products.Where(p => p.CategoryId == id).ToList();
            return View(prolist);
        }

        
        
    }
}
