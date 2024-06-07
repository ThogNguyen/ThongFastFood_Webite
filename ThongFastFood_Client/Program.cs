using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using DinkToPdf.Contracts;
using DinkToPdf;
using ThongFastFood_Client.Services;

namespace ThongFastFood_Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();

            builder.Services.AddDbContext<FoodStoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("db"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                // Password requirements...
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<FoodStoreDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            builder.Services.AddTransient<IPDFService, PDFService>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            builder.Services.AddRazorPages();

            // Add ToastNotification
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 3;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "AdminArea",
                pattern: "{area:exists}/{controller=AdminTrangChu}/{action=MainPage}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=MainPage}/{action=Index}/{id?}");

            app.UseNotyf();

            app.MapRazorPages();
            app.Run();
        }
    }
}
