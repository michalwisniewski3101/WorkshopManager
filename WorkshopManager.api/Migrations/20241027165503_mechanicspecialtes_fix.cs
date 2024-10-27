using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class mechanicspecialtes_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_dict_MechanicSpecialties_SpecialtyId",
                table: "Mechanics");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_SpecialtyId",
                table: "Mechanics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Mechanics_SpecialtyId",
                table: "Mechanics",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_dict_MechanicSpecialties_SpecialtyId",
                table: "Mechanics",
                column: "SpecialtyId",
                principalTable: "dict_MechanicSpecialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
