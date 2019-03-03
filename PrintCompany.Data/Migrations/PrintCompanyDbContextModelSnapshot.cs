﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrintCompany.Data;

namespace PrintCompany.Data.Migrations
{
    [DbContext(typeof(PrintCompanyDbContext))]
    partial class PrintCompanyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PrintCompany.Core.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BillingAddress");

                    b.Property<string>("MainContact");

                    b.Property<string>("Name");

                    b.Property<string>("ShippingAddress");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("PrintCompany.Core.ItemColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color");

                    b.HasKey("Id");

                    b.ToTable("ItemColors");
                });

            modelBuilder.Entity("PrintCompany.Core.ItemSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Size");

                    b.HasKey("Id");

                    b.ToTable("ItemSizes");
                });

            modelBuilder.Entity("PrintCompany.Core.ItemType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("ItemTypes");
                });

            modelBuilder.Entity("PrintCompany.Core.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId");

                    b.Property<DateTime?>("DueDate");

                    b.Property<DateTime>("OrderDate");

                    b.Property<int>("OrderNumber");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PrintCompany.Core.OrderLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("EmbroideryRequired");

                    b.Property<int>("ItemColorId");

                    b.Property<int>("ItemSizeId");

                    b.Property<int>("ItemTypeId");

                    b.Property<int>("OrderId");

                    b.Property<bool>("PrintRequired");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("ItemColorId");

                    b.HasIndex("ItemSizeId");

                    b.HasIndex("ItemTypeId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("PrintCompany.Core.Order", b =>
                {
                    b.HasOne("PrintCompany.Core.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PrintCompany.Core.OrderLine", b =>
                {
                    b.HasOne("PrintCompany.Core.ItemColor", "ItemColor")
                        .WithMany()
                        .HasForeignKey("ItemColorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PrintCompany.Core.ItemSize", "ItemSize")
                        .WithMany()
                        .HasForeignKey("ItemSizeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PrintCompany.Core.ItemType", "ItemType")
                        .WithMany()
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PrintCompany.Core.Order", "Order")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
