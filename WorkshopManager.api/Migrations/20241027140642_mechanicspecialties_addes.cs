using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class mechanicspecialties_addes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Mechanics");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialtyId",
                table: "Mechanics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "dict_MechanicSpecialties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialtyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dict_MechanicSpecialties", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_dict_MechanicSpecialties_SpecialtyId",
                table: "Mechanics");

            migrationBuilder.DropTable(
                name: "dict_MechanicSpecialties");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_SpecialtyId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Mechanics");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Mechanics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
