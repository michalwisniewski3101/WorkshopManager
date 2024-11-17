using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.api.Migrations
{
    /// <inheritdoc />
    public partial class orderUpdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ServiceDuration",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ServiceDuration",
                table: "Services",
                type: "time",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
