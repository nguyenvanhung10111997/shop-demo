using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace example.order.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialExampleDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ORD");

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "ORD",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                schema: "ORD",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                schema: "ORD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTemp",
                schema: "ORD",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    TempData = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTemp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderCode_IsDeleted",
                schema: "ORD",
                table: "Order",
                columns: new[] { "OrderCode", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderStatusId_IsDeleted",
                schema: "ORD",
                table: "Order",
                columns: new[] { "OrderStatusId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTemp_OrderCode_IsDeleted",
                schema: "ORD",
                table: "OrderTemp",
                columns: new[] { "OrderCode", "IsDeleted" },
                unique: true);

            InitOrderStatusData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order",
                schema: "ORD");

            migrationBuilder.DropTable(
                name: "OrderDetail",
                schema: "ORD");

            migrationBuilder.DropTable(
                name: "OrderStatus",
                schema: "ORD");

            migrationBuilder.DropTable(
                name: "OrderTemp",
                schema: "ORD");
        }

        private void InitOrderStatusData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ORD",
                table: "OrderStatus",
                columns: new[] { "Id", "StatusCode", "StatusName", "Description", "CreatedAt", "CreatedBy", "IsDeleted" },
                values: new object[,]
                {
                    {
                        1,
                        "PENDING",
                        "Pending",
                        "Order has been created but not yet processed.",
                        DateTime.Now,
                        "system",
                        false
                    },
                    {
                        2,
                        "PROCESSING",
                        "Processing",
                        "Order is processed",
                        DateTime.Now,
                        "system",
                        false
                    },
                    {
                        3,
                        "PICKED",
                        "Picked",
                        "Order has been picked and is being prepared for delivery.",
                        DateTime.Now,
                        "system",
                        false
                    },
                    {
                        4,
                        "DELIVERING",
                        "Delivering",
                        "Order is currently being delivered to the customer.",
                        DateTime.Now,
                        "system",
                        false
                    },
                    {
                        5,
                        "COMPLETED",
                        "Completed",
                        "Order has been successfully delivered and completed.",
                        DateTime.Now,
                        "system",
                        false
                    },
                    {
                        6,
                        "CANCELLED",
                        "Cancelled",
                        "Order has been cancelled.",
                        DateTime.Now,
                        "system",
                        false
                    }
                });
        }
    }
}
