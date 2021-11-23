using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathTasks.Data.Migrations
{
    public partial class AddedEntitryNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateddBy",
                table: "MathTasks");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MathTasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MathTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Nofifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    AddressFrom = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AddressTo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nofifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nofifications");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MathTasks");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MathTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "UpdateddBy",
                table: "MathTasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
