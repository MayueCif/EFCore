using EFCoreWeb.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreWeb
{
    public class DataDBContext : DbContext
    {

        public DataDBContext(DbContextOptions<DataDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// 订单
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Order>().HasKey(a=>a.Id);
            modelBuilder.Entity<Order>().Property(a => a.ExpressNo).HasMaxLength(20);
            modelBuilder.Entity<Order>().Property(a => a.OrderNo).HasMaxLength(20);
            modelBuilder.Entity<Order>().Property(a => a.Remarks).HasMaxLength(100);
            modelBuilder.Entity<Order>().HasKey(a => a.Id);
            modelBuilder.Entity<Order>().HasIndex(a => new { a.CustomerId, a.CreatedTime }).HasDatabaseName("idx_CustomerId_CreatedTime");
            modelBuilder.Entity<Order>().HasOne(a => a.Customer).WithMany().HasForeignKey(a => a.CustomerId);

            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<OrderItem>().HasKey(a=>a.Id);
            modelBuilder.Entity<OrderItem>().HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .IsRequired();
            modelBuilder.Entity<OrderItem>().Property(a => a.Property1).HasMaxLength(20);
            modelBuilder.Entity<OrderItem>().Property(a => a.Property2).HasMaxLength(20);
            modelBuilder.Entity<OrderItem>().Property(a => a.Property3).HasMaxLength(20);
            modelBuilder.Entity<OrderItem>().Property(a => a.Property4).HasMaxLength(20);
            modelBuilder.Entity<OrderItem>().Property(a => a.Property5).HasMaxLength(20);
            modelBuilder.Entity<OrderItem>().Property(a => a.Property6).HasMaxLength(20);

            modelBuilder.Entity<OrderItem>().HasIndex(a => new { a.ProductId, a.Price, a.Quantity })
                .HasDatabaseName("idx_ProductId_Quantity_Price");

            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Customer>().HasKey(a => a.Id);
            modelBuilder.Entity<Customer>().Property(a => a.Name).HasMaxLength(20);
        }

    }
}
