using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using BulkyWeb.Repository.IRepository;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using BulkyWeb.Repository;
using Bulky.DataAccess.Services;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Microsoft.Extensions.Options;
using BulkyWeb.Areas.Admin.Controllers;

public class Program
{
    public static void Main(string[] args)
    {


        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddSingleton<IEmailSender, EmailSender>();
        builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        builder.Services.AddDbContext<ApplicationDbContext>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<CartService>();
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<HttpClient>();
        StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
            options.Cookie.IsEssential = true; // Make the session cookie essential
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.MapRazorPages();
        app.MapControllerRoute(
            name: "default",
            pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");

        app.Run();

    }
}