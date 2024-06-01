using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;

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

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                // Cấu hình các yêu cầu về mật khẩu
                options.Password.RequireDigit = false;          // Không yêu cầu chứa ký số
                options.Password.RequiredLength = 6;           // Độ dài tối thiểu là 6 ký tự
                options.Password.RequireLowercase = false;     // Không yêu cầu chứa ký tự viết thường
                options.Password.RequireUppercase = false;     // Không yêu cầu chứa ký tự viết hoa
                options.Password.RequireNonAlphanumeric = false; // Không yêu cầu chứa ký tự đặc biệt
            })
            .AddEntityFrameworkStores<FoodStoreDbContext>();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
