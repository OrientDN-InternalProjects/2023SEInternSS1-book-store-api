using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookEcommerce.Migrations
{
    public partial class _020320236h14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_ProductVariants_ProductVariantId",
                table: "CartDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_ProductVariantId",
                table: "CartDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "CartDetailId",
                table: "ProductVariants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_CartDetailId",
                table: "ProductVariants",
                column: "CartDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_CartDetails_CartDetailId",
                table: "ProductVariants",
                column: "CartDetailId",
                principalTable: "CartDetails",
                principalColumn: "CartDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_CartDetails_CartDetailId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_CartDetailId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "CartDetailId",
                table: "ProductVariants");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_ProductVariantId",
                table: "CartDetails",
                column: "ProductVariantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_ProductVariants_ProductVariantId",
                table: "CartDetails",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
