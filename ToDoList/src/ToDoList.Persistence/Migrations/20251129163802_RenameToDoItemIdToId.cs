using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameToDoItemIdToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToDoItemId",
                table: "ToDoItems",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ToDoItems",
                newName: "ToDoItemId");
        }
    }
}
