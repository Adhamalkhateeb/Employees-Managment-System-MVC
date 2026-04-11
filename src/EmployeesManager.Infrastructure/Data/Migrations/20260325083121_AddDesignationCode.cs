using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDesignationCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Designations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Code",
                table: "Designations",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Designations_Code",
                table: "Designations");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Designations");
        }
    }
}
