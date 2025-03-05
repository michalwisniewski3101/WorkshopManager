using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class TaxRate_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaxRate",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "InventoryItems");
        }
    }
}
