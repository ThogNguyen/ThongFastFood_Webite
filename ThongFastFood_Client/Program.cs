using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;

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

            app.UseAuthorization();

			app.MapControllerRoute(
				name: "AdminArea",
				pattern: "{area:exists}/{controller=AdminTrangChu}/{action=MainPage}/{id?}");

			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=MainPage}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
