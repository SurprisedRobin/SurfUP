using Microsoft.EntityFrameworkCore;
using SurfUPWeb.Data;
using Microsoft.AspNetCore.Identity;
using SurfUPWeb.Areas.Identity.Data;
using Microsoft.VisualBasic;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UserDbContextConnection");

builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUsers>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserDbContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MVCSurfUpDB>(options => 
options.UseSqlServer(builder.Configuration
.GetConnectionString("MVCSurfUpDBConnectionString")));

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserDbContext>(options =>
options.UseSqlServer(builder.Configuration
.GetConnectionString("UserDbContextConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

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
app.UseAuthentication();;
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();