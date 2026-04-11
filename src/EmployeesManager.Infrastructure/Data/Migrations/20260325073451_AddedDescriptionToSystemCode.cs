using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescriptionToSystemCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_SystemCodes_Name", table: "SystemCodes");

            migrationBuilder.DropColumn(name: "Name", table: "SystemCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SystemCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50
            );

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SystemCodes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SystemCodeDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500
            );

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SystemCodeDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Description", table: "SystemCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SystemCodes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100
            );

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SystemCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SystemCodeDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SystemCodeDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100
            );

            migrationBuilder.CreateIndex(
                name: "IX_SystemCodes_Name",
                table: "SystemCodes",
                column: "Name",
                unique: true
            );
        }
    }
}
