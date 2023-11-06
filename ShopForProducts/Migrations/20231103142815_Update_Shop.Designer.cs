﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShopForProducts.Entities;

#nullable disable

namespace ShopForProducts.Migrations
{
    [DbContext(typeof(AppDbcontext))]
    [Migration("20231103142815_Update_Shop")]
    partial class Update_Shop
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShopForProducts.Entities.Account", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DecentralizationId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResetPasswordTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("User_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.HasIndex("DecentralizationId");

                    b.ToTable("dboAccounts");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Cart_item", b =>
                {
                    b.Property<int>("Cart_itenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Cart_itenId"));

                    b.Property<int?>("CartId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<int?>("quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Cart_itenId");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("dboCart_item");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Carts", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartId"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("CartId");

                    b.HasIndex("UserId");

                    b.ToTable("dboCrats");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Decentralization", b =>
                {
                    b.Property<int>("DecentralizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DecentralizationId"));

                    b.Property<string>("Authority_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("DecentralizationId");

                    b.ToTable("dboDecentralizations");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Order_statusId")
                        .HasColumnType("int");

                    b.Property<int?>("PaymentId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<double?>("actual_price")
                        .HasColumnType("float");

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("full_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("original_price")
                        .HasColumnType("float");

                    b.Property<string>("phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderId");

                    b.HasIndex("Order_statusId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("dboOrders");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Order_detail", b =>
                {
                    b.Property<int>("Order_detailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Order_detailId"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<double?>("price_total")
                        .HasColumnType("float");

                    b.Property<int?>("quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Order_detailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("dboOrder_detail");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Order_status", b =>
                {
                    b.Property<int>("Order_statusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Order_statusId"));

                    b.Property<string>("status_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Order_statusId");

                    b.ToTable("dboOrder_status");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("payment_method")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("PaymentId");

                    b.ToTable("dboPayment");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int?>("Product_typeId")
                        .HasColumnType("int");

                    b.Property<string>("avartar_image_product")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<int?>("discount")
                        .HasColumnType("int");

                    b.Property<string>("name_product")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("number_of_views")
                        .HasColumnType("int");

                    b.Property<double?>("price")
                        .HasColumnType("float");

                    b.Property<int?>("status")
                        .HasColumnType("int");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("ProductId");

                    b.HasIndex("Product_typeId");

                    b.ToTable("dboProducts");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Product_review", b =>
                {
                    b.Property<int>("Product_reviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Product_reviewId"));

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("content_rated")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("content_seen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<int?>("point_evaluation")
                        .HasColumnType("int");

                    b.Property<int?>("status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Product_reviewId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("dboProduct_Reviews");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Product_type", b =>
                {
                    b.Property<int>("Product_typeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Product_typeId"));

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("name_product_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Product_typeId");

                    b.ToTable("dboProduct_Types");
                });

            modelBuilder.Entity("ShopForProducts.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("ShopForProducts.Entities.VnPay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("OrderDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VnPayResponseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("vnPays");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Account", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Decentralization", "decentralizations")
                        .WithMany("Accounts")
                        .HasForeignKey("DecentralizationId");

                    b.Navigation("decentralizations");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Cart_item", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Carts", "cart")
                        .WithMany("Cart_Items")
                        .HasForeignKey("CartId");

                    b.HasOne("ShopForProducts.Entities.Product", "product")
                        .WithMany("cart_Items")
                        .HasForeignKey("ProductId");

                    b.Navigation("cart");

                    b.Navigation("product");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Carts", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Account", "Account")
                        .WithMany("carts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Order", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Order_status", "status")
                        .WithMany("orders")
                        .HasForeignKey("Order_statusId");

                    b.HasOne("ShopForProducts.Entities.Payment", "Payment")
                        .WithMany("orders")
                        .HasForeignKey("PaymentId");

                    b.HasOne("ShopForProducts.Entities.Account", "Account")
                        .WithMany("orders")
                        .HasForeignKey("UserId");

                    b.Navigation("Account");

                    b.Navigation("Payment");

                    b.Navigation("status");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Order_detail", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Order", "Order")
                        .WithMany("order_detail")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShopForProducts.Entities.Product", "Product")
                        .WithMany("Order_Details")
                        .HasForeignKey("ProductId");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Product", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Product_type", "product_Type")
                        .WithMany("products")
                        .HasForeignKey("Product_typeId");

                    b.Navigation("product_Type");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Product_review", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Product", "Product")
                        .WithMany("product_Reviews")
                        .HasForeignKey("ProductId");

                    b.HasOne("ShopForProducts.Entities.Account", "Account")
                        .WithMany("product_Reviews")
                        .HasForeignKey("UserId");

                    b.Navigation("Account");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ShopForProducts.Entities.RefreshToken", b =>
                {
                    b.HasOne("ShopForProducts.Entities.Account", "AccountUsrtId")
                        .WithMany("refreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountUsrtId");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Account", b =>
                {
                    b.Navigation("carts");

                    b.Navigation("orders");

                    b.Navigation("product_Reviews");

                    b.Navigation("refreshTokens");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Carts", b =>
                {
                    b.Navigation("Cart_Items");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Decentralization", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Order", b =>
                {
                    b.Navigation("order_detail");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Order_status", b =>
                {
                    b.Navigation("orders");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Payment", b =>
                {
                    b.Navigation("orders");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Product", b =>
                {
                    b.Navigation("Order_Details");

                    b.Navigation("cart_Items");

                    b.Navigation("product_Reviews");
                });

            modelBuilder.Entity("ShopForProducts.Entities.Product_type", b =>
                {
                    b.Navigation("products");
                });
#pragma warning restore 612, 618
        }
    }
}
