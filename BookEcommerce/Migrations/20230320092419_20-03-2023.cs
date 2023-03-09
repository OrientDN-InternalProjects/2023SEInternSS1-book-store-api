using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookEcommerce.Migrations
{
    public partial class _20032023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PruductVariantSalePrice",
                table: "ProductPrices",
                newName: "ProductVariantSalePrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductVariantSalePrice",
                table: "ProductPrices",
                newName: "PruductVariantSalePrice");
        }
    }
}
