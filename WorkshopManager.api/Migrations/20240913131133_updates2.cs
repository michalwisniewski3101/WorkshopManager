using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class updates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Orders_OrderId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_Orders_OrderId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Orders_OrderId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_OrderId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ServiceDate",
                table: "ServiceSchedules",
                newName: "ServiceDateStart");

            migrationBuilder.RenameColumn(
                name: "MechanicId",
                table: "ServiceSchedules",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Mechanics",
                newName: "ServiceScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Mechanics_OrderId",
                table: "Mechanics",
                newName: "IX_Mechanics_ServiceScheduleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "ServiceSchedules",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "ServiceDateEnd",
                table: "ServiceSchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceScheduleId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNumber",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedules_OrderId",
                table: "ServiceSchedules",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedules_ServiceId",
                table: "ServiceSchedules",
                column: "ServiceId");

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
                name: "FK_OrderItems_ServiceSchedules_ServiceScheduleId",
                table: "OrderItems",
                column: "ServiceScheduleId",
                principalTable: "ServiceSchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_Orders_OrderId",
                table: "ServiceSchedules",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSchedules_Services_ServiceId",
                table: "ServiceSchedules",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_ServiceSchedules_ServiceScheduleId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ServiceSchedules_ServiceScheduleId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_Orders_OrderId",
                table: "ServiceSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSchedules_Services_ServiceId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSchedules_OrderId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSchedules_ServiceId",
                table: "ServiceSchedules");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ServiceScheduleId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ServiceDateEnd",
                table: "ServiceSchedules");

            migrationBuilder.DropColumn(
                name: "ServiceScheduleId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductNumber",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "ServiceSchedules",
                newName: "MechanicId");

            migrationBuilder.RenameColumn(
                name: "ServiceDateStart",
                table: "ServiceSchedules",
                newName: "ServiceDate");

            migrationBuilder.RenameColumn(
                name: "ServiceScheduleId",
                table: "Mechanics",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Mechanics_ServiceScheduleId",
                table: "Mechanics",
                newName: "IX_Mechanics_OrderId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "ServiceSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Services_OrderId",
                table: "Services",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Orders_OrderId",
                table: "Invoices",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_Orders_OrderId",
                table: "Mechanics",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Orders_OrderId",
                table: "Services",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
