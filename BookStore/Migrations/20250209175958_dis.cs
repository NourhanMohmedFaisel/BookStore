using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class dis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Discounts_DiscountId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Books_DiscountId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Books");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Books",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_DiscountId",
                table: "Books",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Discounts_DiscountId",
                table: "Books",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");
        }
    }
}
