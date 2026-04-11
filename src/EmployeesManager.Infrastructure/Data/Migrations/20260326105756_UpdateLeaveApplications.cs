using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeaveApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApplications_SystemCodeDetails_StatusId",
                table: "LeaveApplications"
            );

            migrationBuilder.DropIndex(
                name: "IX_LeaveApplications_StatusId",
                table: "LeaveApplications"
            );

            migrationBuilder.DropColumn(name: "StatusId", table: "LeaveApplications");

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "LeaveApplications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LeaveApplications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_Status",
                table: "LeaveApplications",
                column: "Status"
            );

            migrationBuilder.AddCheckConstraint(
                name: "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                table: "LeaveApplications",
                sql: "[EndDate] >= [StartDate]"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveApplications_Status",
                table: "LeaveApplications"
            );

            migrationBuilder.DropCheckConstraint(
                name: "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                table: "LeaveApplications"
            );

            migrationBuilder.DropColumn(name: "RejectionReason", table: "LeaveApplications");

            migrationBuilder.DropColumn(name: "Status", table: "LeaveApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "LeaveApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_StatusId",
                table: "LeaveApplications",
                column: "StatusId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_SystemCodeDetails_StatusId",
                table: "LeaveApplications",
                column: "StatusId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
