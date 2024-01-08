using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedValuefieldToCategoriesAndSubcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Subcategories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Categories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Categories");
        }
    }
}
