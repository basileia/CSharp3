using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ToDoItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_CategoryId",
                table: "ToDoItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_Categories_CategoryId",
                table: "ToDoItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_Categories_CategoryId",
                table: "ToDoItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_CategoryId",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ToDoItems");
        }
    }
}
