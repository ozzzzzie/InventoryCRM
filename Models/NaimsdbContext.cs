using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NAIMS.Models;

public partial class NaimsdbContext : DbContext
{
  public NaimsdbContext()
  {
  }

  public NaimsdbContext(DbContextOptions<NaimsdbContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Brand> Brands { get; set; }

  public virtual DbSet<Contact> Contacts { get; set; }

  public virtual DbSet<Order> Orders { get; set; }

  public virtual DbSet<Product> Products { get; set; }

  public virtual DbSet<ProductsOrder> ProductsOrders { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {

  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Brand>(entity =>
    {
      entity.HasKey(e => e.BrandId).HasName("PK__BRANDS__F89D34097EBADCE0");

      entity.ToTable("BRANDS");

      entity.HasIndex(e => e.BrandId, "UQ__BRANDS__F89D3408A1095A56").IsUnique();

      entity.Property(e => e.BrandId).HasColumnName("BRAND_ID");
      entity.Property(e => e.Bdescription)
              .HasMaxLength(1024)
              .IsUnicode(false)
              .HasColumnName("BDESCRIPTION");
      entity.Property(e => e.Bname)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("BNAME");
    });

    modelBuilder.Entity<Contact>(entity =>
    {
      entity.HasKey(e => e.ContactId).HasName("PK__CONTACTS__99DE4258F3160471");

      entity.ToTable("CONTACTS");

      entity.HasIndex(e => e.ContactId, "UQ__CONTACTS__99DE4259510B8AAE").IsUnique();

      entity.Property(e => e.ContactId).HasColumnName("CONTACT_ID");
      entity.Property(e => e.Caddress)
              .HasMaxLength(256)
              .IsUnicode(false)
              .HasColumnName("CADDRESS");
      entity.Property(e => e.Cname)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("CNAME");
      entity.Property(e => e.Ctype)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("CTYPE");
      entity.Property(e => e.Email)
              .HasMaxLength(128)
              .IsUnicode(false)
              .HasColumnName("EMAIL");
      entity.Property(e => e.PhoneNumber)
              .HasMaxLength(128)
              .IsUnicode(false)
              .HasColumnName("PHONE_NUMBER");
    });

    modelBuilder.Entity<Order>(entity =>
    {
      entity.HasKey(e => e.OrderId).HasName("PK__ORDERS__460A9464B3C365C6");

      entity.ToTable("ORDERS");

      entity.HasIndex(e => e.OrderId, "UQ__ORDERS__460A946581728E4A").IsUnique();

      entity.Property(e => e.OrderId).HasColumnName("ORDER_ID");
      entity.Property(e => e.ContactId).HasColumnName("CONTACT_ID");
      entity.Property(e => e.OrderDate).HasColumnName("ORDER_DATE");
      entity.Property(e => e.SalesRep)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("SALES_REP");

      entity.HasOne(d => d.Contact).WithMany(p => p.Orders)
              .HasForeignKey(d => d.ContactId)
              .HasConstraintName("FK__ORDERS__CONTACT___2F10007B");
    });

    modelBuilder.Entity<Product>(entity =>
    {
      entity.HasKey(e => e.ProductId).HasName("PK__PRODUCTS__52B417630C00E652");

      entity.ToTable("PRODUCTS");

      entity.HasIndex(e => e.ProductId, "UQ__PRODUCTS__52B4176228A29EF0").IsUnique();

      entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
      entity.Property(e => e.Barcode).HasColumnName("BARCODE");
      entity.Property(e => e.BrandId).HasColumnName("BRAND_ID");
      entity.Property(e => e.LocalQty).HasColumnName("LOCAL_QTY");
      entity.Property(e => e.LocalStatus)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("LOCAL_STATUS");
      entity.Property(e => e.Pdescription)
              .HasMaxLength(1024)
              .IsUnicode(false)
              .HasColumnName("PDESCRIPTION");
      entity.Property(e => e.Pname)
              .HasMaxLength(256)
              .IsUnicode(false)
              .HasColumnName("PNAME");
      entity.Property(e => e.Price).HasColumnName("PRICE");
      entity.Property(e => e.PriceVat).HasColumnName("PRICE_VAT");
      entity.Property(e => e.Size)
              .HasMaxLength(32)
              .IsUnicode(false)
              .HasColumnName("SIZE");
      entity.Property(e => e.Sku)
              .HasMaxLength(32)
              .IsUnicode(false)
              .HasColumnName("SKU");
      entity.Property(e => e.WarehouseQty).HasColumnName("WAREHOUSE_QTY");
      entity.Property(e => e.WarehouseStatus)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("WAREHOUSE_STATUS");

      entity.HasOne(d => d.Brand).WithMany(p => p.Products)
              .HasForeignKey(d => d.BrandId)
              .HasConstraintName("FK__PRODUCTS__BRAND___286302EC");
    });

    modelBuilder.Entity<ProductsOrder>(entity =>
    {
      entity.HasKey(e => e.ProductorderId).HasName("PK__PRODUCTS__2C75A0F2E0F0A37C");

      entity.ToTable("PRODUCTS_ORDERS");

      entity.HasIndex(e => e.ProductorderId, "UQ__PRODUCTS__2C75A0F35BCA2BA8").IsUnique();

      entity.Property(e => e.ProductorderId).HasColumnName("PRODUCTORDER_ID");
      entity.Property(e => e.OrderId).HasColumnName("ORDER_ID");
      entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
      entity.Property(e => e.Qty).HasColumnName("QTY");
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
