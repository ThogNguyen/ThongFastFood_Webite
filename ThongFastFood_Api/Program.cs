using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Repositories.CategoryService;
using ThongFastFood_Api.Repositories.ProductService;
using ThongFastFood_Api.Repositories.UserService;

namespace ThongFastFood_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FoodStoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("db"));
            });

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;

				// Cấu hình các yêu cầu về mật khẩu
				options.Password.RequireDigit = false;          // Không yêu cầu chứa ký số
				options.Password.RequiredLength = 6;           // Độ dài tối thiểu là 6 ký tự
				options.Password.RequireLowercase = false;     // Không yêu cầu chứa ký tự viết thường
				options.Password.RequireUppercase = false;     // Không yêu cầu chứa ký tự viết hoa
				options.Password.RequireNonAlphanumeric = false; // Không yêu cầu chứa ký tự đặc biệt
			})
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<FoodStoreDbContext>()
			.AddDefaultTokenProviders();

			builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserRoleService, UserRoleService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
