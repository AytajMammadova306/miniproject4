using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniProject.DAL;
using MiniProject.Models;
using MiniProject.Services.Implementations;
using MiniProject.Services.Interfaces;

namespace MiniProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Defualt"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;

                opt.User.RequireUniqueEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<ILayoutService, LayoutService>();
            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllerRoute(
                "defualt",
                "{area:exists}/{controller=product}/{action=index}/{id?}");//muellim adminin homunu front to back elemek istemedim onsuzda isltmirik. ona gore controlleri product verdim birbasa
            app.MapControllerRoute(
                "defualt",
                "{controller=home}/{action=index}/{id?}");
            app.Run();
        }
    }
}
