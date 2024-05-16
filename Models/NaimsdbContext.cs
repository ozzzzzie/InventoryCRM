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

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsOrder> ProductsOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=naimsdb;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__BRANDS__F89D3409423E8CA7");

            entity.ToTable("BRANDS");

            entity.HasIndex(e => e.BrandId, "UQ__BRANDS__F89D34084781A4CB").IsUnique();

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
            entity.HasKey(e => e.ContactId).HasName("PK__CONTACTS__99DE425824EDF05E");

            entity.ToTable("CONTACTS");

            entity.HasIndex(e => e.ContactId, "UQ__CONTACTS__99DE4259DE42EC5E").IsUnique();

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

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__EMPLOYEE__CBA14F4829AE1541");

            entity.ToTable("EMPLOYEES");

            entity.HasIndex(e => e.EmployeeId, "UQ__EMPLOYEE__CBA14F49F37FF8FD").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.EComissionPerc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("E_COMISSION_PERC");
            entity.Property(e => e.EEmail)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("E_EMAIL");
            entity.Property(e => e.EFirstname)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("E_FIRSTNAME");
            entity.Property(e => e.ELastname)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("E_LASTNAME");
            entity.Property(e => e.EPhonenumber)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("E_PHONENUMBER");
            entity.Property(e => e.ERole)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("E_ROLE");
            entity.Property(e => e.ETarget).HasColumnName("E_TARGET");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__ORDERS__460A9464A3C17516");

            entity.ToTable("ORDERS");

            entity.HasIndex(e => e.OrderId, "UQ__ORDERS__460A9465DF14307F").IsUnique();

            entity.Property(e => e.OrderId).HasColumnName("ORDER_ID");
            entity.Property(e => e.ContactId).HasColumnName("CONTACT_ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.OrderDate).HasColumnName("ORDER_DATE");

            entity.HasOne(d => d.Contact).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ContactId)
                .HasConstraintName("FK__ORDERS__CONTACT___31EC6D26");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__ORDERS__EMPLOYEE__32E0915F");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__PRODUCTS__52B417637B7F9857");

            entity.ToTable("PRODUCTS");

            entity.HasIndex(e => e.ProductId, "UQ__PRODUCTS__52B41762BD59B549").IsUnique();

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
            entity.HasKey(e => e.ProductorderId).HasName("PK__PRODUCTS__2C75A0F23787F313");

            entity.ToTable("PRODUCTS_ORDERS");

            entity.HasIndex(e => e.ProductorderId, "UQ__PRODUCTS__2C75A0F33B8A79F3").IsUnique();

            entity.Property(e => e.ProductorderId).HasColumnName("PRODUCTORDER_ID");
            entity.Property(e => e.OrderId).HasColumnName("ORDER_ID");
            entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
            entity.Property(e => e.Qty).HasColumnName("QTY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
