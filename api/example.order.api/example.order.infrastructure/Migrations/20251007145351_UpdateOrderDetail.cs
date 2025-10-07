using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace example.order.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                schema: "ORD",
                table: "OrderDetail",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                schema: "ORD",
                table: "OrderDetail",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                schema: "ORD",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ProductName",
                schema: "ORD",
                table: "OrderDetail");
        }
    }
}
