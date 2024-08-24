using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Oranges.Models;

public partial class Orders : DbContext
{
    public Orders()
    {
    }

    public Orders(DbContextOptions<Orders> options)
        : base(options)
    {
    }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderList> OrderLists { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-GIEQN5M;Database=Orders;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meals__ACF6A63DA116B276");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MealName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Meals)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK__Meals__Restauran__3D5E1FD2");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF2F65F97A");

            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.OrderDescription).HasMaxLength(255);
            entity.Property(e => e.PaidAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.RemainingAmount)
                .HasComputedColumnSql("([TotalPrice]-[PaidAmount])", true)
                .HasColumnType("decimal(11, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Meal).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MealId)
                .HasConstraintName("FK__Orders__MealId__46E78A0C");

            entity.HasOne(d => d.OrderList).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderListId)
                .HasConstraintName("FK__Orders__OrderLis__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__UserId__48CFD27E");
        });

        modelBuilder.Entity<OrderList>(entity =>
        {
            entity.HasKey(e => e.OrderListId).HasName("PK__OrderLis__BFE8AA3BC4B8EC3B");

            entity.Property(e => e.DeliveryPrice)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.OrderListName).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.OrderLists)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK__OrderList__Creat__403A8C7D");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.OrderLists)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK__OrderList__Resta__412EB0B6");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454C9577E1184C");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RestaurantName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C8303B544");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E49254641B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534327D64EC").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
