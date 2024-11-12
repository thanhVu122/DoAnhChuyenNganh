using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");




//dang ki context 
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
