using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopForProducts.Migrations
{
    /// <inheritdoc />
    public partial class Add_Shop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dboDecentralizations",
                columns: table => new
                {
                    DecentralizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Authority_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboDecentralizations", x => x.DecentralizationId);
                });

            migrationBuilder.CreateTable(
                name: "dboOrder_status",
                columns: table => new
                {
                    Order_statusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboOrder_status", x => x.Order_statusId);
                });

            migrationBuilder.CreateTable(
                name: "dboPayment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboPayment", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "dboProduct_Types",
                columns: table => new
                {
                    Product_typeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_product_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboProduct_Types", x => x.Product_typeId);
                });

            migrationBuilder.CreateTable(
                name: "vnPays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VnPayResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vnPays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dboAccounts",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DecentralizationId = table.Column<int>(type: "int", nullable: true),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboAccounts", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_dboAccounts_dboDecentralizations_DecentralizationId",
                        column: x => x.DecentralizationId,
                        principalTable: "dboDecentralizations",
                        principalColumn: "DecentralizationId");
                });

            migrationBuilder.CreateTable(
                name: "dboProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_typeId = table.Column<int>(type: "int", nullable: true),
                    name_product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    avartar_image_product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discount = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    number_of_views = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboProducts", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_dboProducts_dboProduct_Types_Product_typeId",
                        column: x => x.Product_typeId,
                        principalTable: "dboProduct_Types",
                        principalColumn: "Product_typeId");
                });

            migrationBuilder.CreateTable(
                name: "dboCrats",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboCrats", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_dboCrats_dboAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "dboAccounts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dboOrders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    original_price = table.Column<double>(type: "float", nullable: true),
                    actual_price = table.Column<double>(type: "float", nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order_statusId = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboOrders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_dboOrders_dboAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "dboAccounts",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_dboOrders_dboOrder_status_Order_statusId",
                        column: x => x.Order_statusId,
                        principalTable: "dboOrder_status",
                        principalColumn: "Order_statusId");
                    table.ForeignKey(
                        name: "FK_dboOrders_dboPayment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "dboPayment",
                        principalColumn: "PaymentId");
                });

            migrationBuilder.CreateTable(
                name: "dboProduct_Reviews",
                columns: table => new
                {
                    Product_reviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    content_rated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    point_evaluation = table.Column<int>(type: "int", nullable: true),
                    content_seen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboProduct_Reviews", x => x.Product_reviewId);
                    table.ForeignKey(
                        name: "FK_dboProduct_Reviews_dboAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "dboAccounts",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_dboProduct_Reviews_dboProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "dboProducts",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "dboCart_item",
                columns: table => new
                {
                    Cart_itenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    CartId = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboCart_item", x => x.Cart_itenId);
                    table.ForeignKey(
                        name: "FK_dboCart_item_dboCrats_CartId",
                        column: x => x.CartId,
                        principalTable: "dboCrats",
                        principalColumn: "CartId");
                    table.ForeignKey(
                        name: "FK_dboCart_item_dboProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "dboProducts",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "dboOrder_detail",
                columns: table => new
                {
                    Order_detailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    price_total = table.Column<double>(type: "float", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dboOrder_detail", x => x.Order_detailId);
                    table.ForeignKey(
                        name: "FK_dboOrder_detail_dboOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "dboOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dboOrder_detail_dboProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "dboProducts",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_dboAccounts_DecentralizationId",
                table: "dboAccounts",
                column: "DecentralizationId");

            migrationBuilder.CreateIndex(
                name: "IX_dboCart_item_CartId",
                table: "dboCart_item",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_dboCart_item_ProductId",
                table: "dboCart_item",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_dboCrats_UserId",
                table: "dboCrats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dboOrder_detail_OrderId",
                table: "dboOrder_detail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_dboOrder_detail_ProductId",
                table: "dboOrder_detail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_dboOrders_Order_statusId",
                table: "dboOrders",
                column: "Order_statusId");

            migrationBuilder.CreateIndex(
                name: "IX_dboOrders_PaymentId",
                table: "dboOrders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_dboOrders_UserId",
                table: "dboOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dboProduct_Reviews_ProductId",
                table: "dboProduct_Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_dboProduct_Reviews_UserId",
                table: "dboProduct_Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dboProducts_Product_typeId",
                table: "dboProducts",
                column: "Product_typeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dboCart_item");

            migrationBuilder.DropTable(
                name: "dboOrder_detail");

            migrationBuilder.DropTable(
                name: "dboProduct_Reviews");

            migrationBuilder.DropTable(
                name: "vnPays");

            migrationBuilder.DropTable(
                name: "dboCrats");

            migrationBuilder.DropTable(
                name: "dboOrders");

            migrationBuilder.DropTable(
                name: "dboProducts");

            migrationBuilder.DropTable(
                name: "dboAccounts");

            migrationBuilder.DropTable(
                name: "dboOrder_status");

            migrationBuilder.DropTable(
                name: "dboPayment");

            migrationBuilder.DropTable(
                name: "dboProduct_Types");

            migrationBuilder.DropTable(
                name: "dboDecentralizations");
        }
    }
}
