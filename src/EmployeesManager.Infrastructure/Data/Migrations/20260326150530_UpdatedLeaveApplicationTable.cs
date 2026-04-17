using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLeaveApplicationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                table: "LeaveApplications"
            );

            migrationBuilder.DropColumn(name: "ApprovedBy", table: "LeaveApplications");

            migrationBuilder.RenameColumn(
                name: "ApprovedAtUtc",
                table: "LeaveApplications",
                newName: "DecisionAtUtc"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LeaveApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "LeaveApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "DecisionById",
                table: "LeaveApplications",
                type: "uniqueidentifier",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_DecisionById",
                table: "LeaveApplications",
                column: "DecisionById"
            );

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                table: "LeaveApplications",
                sql: "CAST(EndDate AS DATE) >= CAST(StartDate AS DATE)"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveApplications_DecisionById",
                table: "LeaveApplications"
            );

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                table: "LeaveApplications"
            );

            migrationBuilder.DropColumn(name: "DecisionById", table: "LeaveApplications");

            migrationBuilder.RenameColumn(
                name: "DecisionAtUtc",
                table: "LeaveApplications",
                newName: "ApprovedAtUtc"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LeaveApplications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50
            );

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "LeaveApplications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50
            );

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "LeaveApplications",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true
            );

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                table: "LeaveApplications",
                sql: "[EndDate] >= [StartDate]"
            );
        }
    }
}
