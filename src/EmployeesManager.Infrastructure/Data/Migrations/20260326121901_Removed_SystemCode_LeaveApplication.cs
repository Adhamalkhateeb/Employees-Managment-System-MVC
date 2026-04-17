using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Removed_SystemCode_LeaveApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApplications_SystemCodeDetails_DurationId",
                table: "LeaveApplications");

            migrationBuilder.DropIndex(
                name: "IX_LeaveApplications_DurationId",
                table: "LeaveApplications");

            migrationBuilder.DropColumn(
                name: "DurationId",
                table: "LeaveApplications");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "LeaveApplications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_Duration",
                table: "LeaveApplications",
                column: "Duration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveApplications_Duration",
                table: "LeaveApplications");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "LeaveApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "DurationId",
                table: "LeaveApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_DurationId",
                table: "LeaveApplications",
                column: "DurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_SystemCodeDetails_DurationId",
                table: "LeaveApplications",
                column: "DurationId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
