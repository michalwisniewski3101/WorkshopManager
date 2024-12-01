using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class updates5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_Services_ServiceId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSchedules_ServiceId",
                table: "ServiceSchedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedules_ServiceId",
                table: "ServiceSchedules",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_Services_ServiceId",
                table: "ServiceSchedules",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
