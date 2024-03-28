using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Domain.Entities.Users;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugeto_Store.Common.Roles;
using Store.Domain.Entities.HomePage;
using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Finance;
using Store.Domain.Entities.Orders;

namespace Store.Persistance.Contexts
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<HomePageImages> HomePageImages { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<RequestPay> RequestPays { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(p => p.User)
                .WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(p => p.RequestPay)
                .WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.NoAction);


            //Seed Date
            SeedData(modelBuilder);

            //عدم نمایش اطلاعات حذف شده
            ApplyQueryFilter(modelBuilder);

            //یونیک کردن ایمیل
            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();


        }


        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<Role>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<UserInRole>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<Category>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<ProductImages>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<ProductFeature>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<Slider>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<HomePageImages>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<Cart>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<CartItem>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<RequestPay>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<Order>().HasQueryFilter(p => p.IsRemoved != true);
            modelBuilder.Entity<OrderDetail>().HasQueryFilter(p => p.IsRemoved != true);


        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 1, Name = UserRoles.Admin });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 2, Name = UserRoles.Operator });
            modelBuilder.Entity<Role>().HasData(new Role() { Id = 3, Name = UserRoles.Customer });
        }








    }
}
