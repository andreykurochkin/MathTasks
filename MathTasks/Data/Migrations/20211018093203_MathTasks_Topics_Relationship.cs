using Microsoft.EntityFrameworkCore.Migrations;

namespace MathTasks.Data.Migrations
{
    public partial class MathTasks_Topics_Relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "MathTasks");

            migrationBuilder.RenameColumn(
                name: "Theme",
                table: "MathTasks",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "MathTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MathTasks_TopicId",
                table: "MathTasks",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_MathTasks_Topic_TopicId",
                table: "MathTasks",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MathTasks_Topic_TopicId",
                table: "MathTasks");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_MathTasks_TopicId",
                table: "MathTasks");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "MathTasks");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "MathTasks",
                newName: "Theme");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "MathTasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
