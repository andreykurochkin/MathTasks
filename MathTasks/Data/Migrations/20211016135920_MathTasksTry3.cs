using Microsoft.EntityFrameworkCore.Migrations;

namespace MathTasks.Data.Migrations
{
    public partial class MathTasksTry3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "MathTasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "MathTasks");
        }
    }
}
