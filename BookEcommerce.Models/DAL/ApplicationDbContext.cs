using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasOne<Admin>(o => o.Admin)
                .WithOne(o => o.Account)
                .HasForeignKey<Admin>(k => k.AccountId);
            builder.Entity<ApplicationUser>()
                .HasOne<Vendor>(o => o.Vendor)
                .WithOne(o => o.Account)
                .HasForeignKey<Vendor>(k => k.AccountId);
            builder.Entity<ApplicationUser>()
                .HasOne<Customer>(o => o.Customer)
                .WithOne(o => o.Account)
                .HasForeignKey<Customer>(k => k.AccountId);

        }

        //public virtual DbSet<UserAccountRole> UserAccountRoles { get; set; }
        //public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<BankAccount> BankAcocunts { get; set; }
        public virtual DbSet<BankProvider> BankProviders { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductVariant> ProductVariants { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
    }
}
