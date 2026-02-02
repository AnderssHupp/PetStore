using Loja.Models;
using Microsoft.EntityFrameworkCore;

namespace Loja.Data
{
    public class PetStoreContext(DbContextOptions<PetStoreContext> options)
        : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Stock> Stock => Set<Stock>();
        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Shipment> Shipments => Set<Shipment>();

        public DbSet<User> Users => Set<User>();
        public DbSet<Address> Addresses => Set<Address>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                    .HasOne(p => p.Stock)
                    .WithOne(s => s.Product)
                    .HasForeignKey<Stock>(s => s.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(o => o.Order)
                .HasForeignKey<Payment>( p => p.OrderId )
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Order>()
                .HasOne(s => s.Shipment )
                .WithOne(o => o.Order)
                .HasForeignKey<Shipment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);



        }


    }


}
