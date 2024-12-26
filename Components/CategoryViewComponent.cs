using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StationShop.AppData;
using StationShop.Models;


namespace StationShop.Components
{
    public class CategoryViewComponent: ViewComponent
    {
        private readonly AppDBContext _db;
        public CategoryViewComponent(AppDBContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> cateList = await this._db.Categories.ToListAsync();
            return View(cateList);
        }
    }
}
