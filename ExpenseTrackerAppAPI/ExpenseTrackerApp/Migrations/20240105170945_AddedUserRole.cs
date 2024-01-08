using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "UserAccounts",
                newName: "UserRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "UserAccounts",
                newName: "Role");
        }
    }
}
