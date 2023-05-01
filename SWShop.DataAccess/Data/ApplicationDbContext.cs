using SWShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SWShop.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(c => c.Sizes)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Áo", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Quần", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Giày", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Fortune of Time",
                    Brand = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ListPrice = 99,
                    Price = 90,
                    SalePrice = 85,
                    QuantitySold = 0,
                    QuantityRemain = 10,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Dark Skies",
                    Brand = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ListPrice = 99,
                    Price = 90,
                    SalePrice = 85,
                    QuantitySold = 0,
                    QuantityRemain = 10,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 3,
                    Name = "Vanish in the Sunset",
                    Brand = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ListPrice = 99,
                    Price = 90,
                    SalePrice = 85,
                    QuantitySold = 0,
                    QuantityRemain = 10,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 4,
                    Name = "Cotton Candy",
                    Brand = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ListPrice = 99,
                    Price = 90,
                    SalePrice = 85,
                    QuantitySold = 0,
                    QuantityRemain = 10,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 5,
                    Name = "Rock in the Ocean",
                    Brand = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ListPrice = 99,
                    Price = 90,
                    SalePrice = 85,
                    QuantitySold = 0,
                    QuantityRemain = 10,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 6,
                    Name = "Leaves and Wonders",
                    Brand = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ListPrice = 99,
                    Price = 90,
                    SalePrice = 85,
                    QuantitySold = 0,
                    QuantityRemain = 10,
                    CategoryId = 1
                }
                );
        }
    }
}
