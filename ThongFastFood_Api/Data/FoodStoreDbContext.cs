using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ThongFastFood_Api.Data
{
    public class FoodStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public FoodStoreDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Review> Reviews { get; set; }

		/*public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }*/
	}
}
