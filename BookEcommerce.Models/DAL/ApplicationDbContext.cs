using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base()
        {

        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Admin>()
                .HasOne<ApplicationUser>(o => o.Account)
                .WithOne(o => o.Admin)
                .HasForeignKey<Admin>(k => k.AccountId);

            builder.Entity<Vendor>()
                .HasOne<ApplicationUser>(o => o.Account)
                .WithOne(o => o.Vendor)
                .HasForeignKey<Vendor>(k => k.AccountId);

            builder.Entity<Customer>()
                .HasOne<ApplicationUser>(o => o.Account)
                .WithOne(o => o.Customer)
                .HasForeignKey<Customer>(k => k.AccountId);

            builder.Entity<Customer>()
                .HasOne<Cart>(o => o.Cart)
                .WithOne(o => o.Customer)
                .HasForeignKey<Cart>(k => k.CustomerId);

            builder.Entity<Vendor>()
                .HasMany<Order>(m => m.Orders)
                .WithOne(o => o.Vendor)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
                .HasOne<Image>(o => o.Image)
                .WithOne(o => o.Category)
                .HasForeignKey<Image>(k => k.CategoryId);

            builder.Entity<Customer>()
                .HasOne<Image>(o => o.Image)
                .WithOne(o => o.Customer)
                .HasForeignKey<Image>(k => k.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vendor>()
                .HasOne<Image>(o => o.Image)
                .WithOne(o => o.Vendor)
                .HasForeignKey<Image>(k => k.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vendor>()
                .HasMany<Product>(m => m.Products)
                .WithOne(o => o.Vendor)
                .HasForeignKey(k => k.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasMany<Image>(m => m.Images)
                .WithOne(o => o.Product)
                .HasForeignKey(k => k.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductCategory>()
                .HasOne<Product>(o => o.Product)
                .WithMany(o => o.ProductCategories)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductVariant>()
                .HasOne<ProductPrice>(o => o.ProductPrice)
                .WithOne(o => o.ProductVariant)
                .HasForeignKey<ProductPrice>(k => k.ProductPriceId);

            builder.Entity<OrderDetail>()
                .HasOne<ProductVariant>(o => o.ProductVariant)
                .WithOne(o => o.OrderDetail)
                .HasForeignKey<OrderDetail>(k => k.ProductVariantId);
        }

        //public virtual DbSet<UserAccountRole> UserAccountRoles { get; set; }
        //public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<AccountToken> AccountTokens { get; set; }
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
