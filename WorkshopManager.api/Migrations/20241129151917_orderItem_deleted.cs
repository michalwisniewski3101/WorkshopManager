using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class orderItem_deleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_ServiceSchedules_ServiceScheduleId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_Orders_OrderId",
                table: "ServiceSchedules");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSchedules_OrderId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_ServiceScheduleId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ServiceSchedules");

            migrationBuilder.DropColumn(
                name: "ServiceScheduleId",
                table: "Mechanics");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceStatus",
                table: "ServiceSchedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mechanics",
                table: "ServiceSchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    ServiceScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => new { x.ServiceScheduleId, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderItem_ServiceSchedules_ServiceScheduleId",
                        column: x => x.ServiceScheduleId,
                        principalTable: "ServiceSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Mechanics",
                table: "ServiceSchedules");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceStatus",
                table: "ServiceSchedules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "ServiceSchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceScheduleId",
                table: "Mechanics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ServiceScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_ServiceSchedules_ServiceScheduleId",
                        column: x => x.ServiceScheduleId,
                        principalTable: "ServiceSchedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedules_OrderId",
                table: "ServiceSchedules",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Mechanics_ServiceScheduleId",
                table: "Mechanics",
                column: "ServiceScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ServiceScheduleId",
                table: "OrderItems",
                column: "ServiceScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_ServiceSchedules_ServiceScheduleId",
                table: "Mechanics",
                column: "ServiceScheduleId",
                principalTable: "ServiceSchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_Orders_OrderId",
                table: "ServiceSchedules",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
