using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveTypeCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "LeaveTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.Sql(
                "UPDATE [LeaveTypes] "
                    + "SET [Code] = CONCAT('LT-', CONVERT(varchar(36), [Id])) "
                    + "WHERE [Code] IS NULL OR LTRIM(RTRIM([Code])) = '';"
            );

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_Code",
                table: "LeaveTypes",
                column: "Code",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_LeaveTypes_Code", table: "LeaveTypes");

            migrationBuilder.DropColumn(name: "Code", table: "LeaveTypes");
        }
    }
}
