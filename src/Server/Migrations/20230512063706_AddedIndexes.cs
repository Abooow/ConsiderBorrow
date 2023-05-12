using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsiderBorrow.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LibraryItems_Type",
                schema: "ResourceManagement",
                table: "LibraryItems",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IsCEO_IsManager",
                schema: "Identity",
                table: "Employees",
                columns: new[] { "IsCEO", "IsManager" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LibraryItems_Type",
                schema: "ResourceManagement",
                table: "LibraryItems");

            migrationBuilder.DropIndex(
                name: "IX_Employees_IsCEO_IsManager",
                schema: "Identity",
                table: "Employees");
        }
    }
}
