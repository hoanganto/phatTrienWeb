using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StationShop.AppData;
using StationShop.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppConnection") ?? throw new InvalidOperationException("Connection string 'IdenityContextConnection' not found.");

builder.Services.AddDbContext<IdenityContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<IdenityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Add this line to include Razor Pages services
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();// nếu không có dòng này thì login thành công vẫn không hiển thị được trạng thái đã đăng nhập
app.UseAuthorization();
//app.MapAreaControllerRoute (
//    name: "defaultArea",
//    areaName: "admin",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
