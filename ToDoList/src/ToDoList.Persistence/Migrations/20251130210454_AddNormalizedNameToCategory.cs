using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedNameToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Categories",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Categories");
        }
    }
}
