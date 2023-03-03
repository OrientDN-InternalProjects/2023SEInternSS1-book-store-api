using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookEcommerce.Migrations
{
    public partial class _01032023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductVariantId",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductVariantId",
                table: "OrderDetails",
                column: "ProductVariantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductVariantId",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductVariantId",
                table: "OrderDetails",
                column: "ProductVariantId",
                unique: true,
                filter: "[ProductVariantId] IS NOT NULL");
        }
    }
}
