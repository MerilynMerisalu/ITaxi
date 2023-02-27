using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteOnBookingDriveAndComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Drives_DriveId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Drives_DriveId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "NumberOfRideTimes",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "NumberOfTakenRideTimes",
                table: "Schedules");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Drives_DriveId",
                table: "Bookings",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Drives_DriveId",
                table: "Comments",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Drives_DriveId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Drives_DriveId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRideTimes",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTakenRideTimes",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Drives_DriveId",
                table: "Bookings",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Drives_DriveId",
                table: "Comments",
                column: "DriveId",
                principalTable: "Drives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
