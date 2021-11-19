using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathTasks.Data.Migrations
{
    public partial class RenameTableFactsToMathTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MathTaskTag_Facts_MathTasksId",
                table: "MathTaskTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facts",
                table: "Facts");

            migrationBuilder.RenameTable(
                name: "Facts",
                newName: "MathTasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MathTasks",
                table: "MathTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MathTaskTag_MathTasks_MathTasksId",
                table: "MathTaskTag",
                column: "MathTasksId",
                principalTable: "MathTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MathTaskTag_MathTasks_MathTasksId",
                table: "MathTaskTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MathTasks",
                table: "MathTasks");

            migrationBuilder.RenameTable(
                name: "MathTasks",
                newName: "Facts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facts",
                table: "Facts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MathTaskTag_Facts_MathTasksId",
                table: "MathTaskTag",
                column: "MathTasksId",
                principalTable: "Facts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
