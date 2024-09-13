using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_InventoryItems_InventoryItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_Mechanics_MechanicId",
                table: "ServiceSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_Orders_OrderId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSchedules_MechanicId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSchedules_OrderId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_InventoryItemId",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "VIN",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Mechanics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mechanics_OrderId",
                table: "Mechanics",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_Orders_OrderId",
                table: "Mechanics",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_Orders_OrderId",
                table: "Mechanics");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_OrderId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "VIN",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Mechanics");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedules_MechanicId",
                table: "ServiceSchedules",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedules_OrderId",
                table: "ServiceSchedules",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_InventoryItemId",
                table: "OrderItems",
                column: "InventoryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_InventoryItems_InventoryItemId",
                table: "OrderItems",
                column: "InventoryItemId",
                principalTable: "InventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_Mechanics_MechanicId",
                table: "ServiceSchedules",
                column: "MechanicId",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_Orders_OrderId",
                table: "ServiceSchedules",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
