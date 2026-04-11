using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmployeeCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Employees_DateOfBirth",
                table: "Employees",
                sql: "DateOfBirth <= DATEADD(year, -16, GETDATE())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Employees_DateOfBirth",
                table: "Employees");
        }
    }
}
