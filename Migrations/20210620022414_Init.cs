using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace EFCoreWeb.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    OrderNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    OrderTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PaymentTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpressNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Property1 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Property2 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Property3 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Property4 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Property5 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Property6 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
