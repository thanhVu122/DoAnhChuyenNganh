using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    // Clears the default area view location formats.
    options.AreaViewLocationFormats.Clear();

    // Adds custom view location formats for areas and shared views.
   
    options.AreaViewLocationFormats.Add("/Areas/Admin/Views/Shared/_ViewImports.cshtml");
   
});
//connection database
builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:myconn"]);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
//add session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.IsEssential = true;
}
);
builder.Services.AddIdentity<AppNguoiDung,IdentityRole>()
    .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;


    //// Lockout settings.
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    //options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});
var app = builder.Build();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statuscode={0}");

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "DanhMuc",
    pattern: "DanhMuc/{MaDanhMuc?}",
    defaults: new {  controller = "DanhMuc", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



//dang ki context 
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
