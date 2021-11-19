using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathTasks.Data.Migrations
{
    public partial class NewFieldsForEntitiesMathTaskAndTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Facts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Facts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Facts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateddBy",
                table: "Facts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "UpdateddBy",
                table: "Facts");
        }
    }
}
