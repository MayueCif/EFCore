using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreWeb.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_CustomerId_CreatedTime",
                table: "Orders",
                columns: new[] { "CustomerId", "CreatedTime" });

            migrationBuilder.CreateIndex(
                name: "idx_ProductId_Quantity_Price",
                table: "OrderItems",
                columns: new[] { "ProductId", "Price", "Quantity" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "idx_CustomerId_CreatedTime",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "idx_ProductId_Quantity_Price",
                table: "OrderItems");
        }
    }
}
