using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class AddedInformationPropertiesToTheDriveEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DriveAcceptedDateAndTime",
                table: "Drives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DriveDeclineDateAndTime",
                table: "Drives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DriveEndDateAndTime",
                table: "Drives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DriveStartDateAndTime",
                table: "Drives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDriveAccepted",
                table: "Drives",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDriveDeclined",
                table: "Drives",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDriveFinished",
                table: "Drives",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDriveStarted",
                table: "Drives",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriveAcceptedDateAndTime",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "DriveDeclineDateAndTime",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "DriveEndDateAndTime",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "DriveStartDateAndTime",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "IsDriveAccepted",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "IsDriveDeclined",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "IsDriveFinished",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "IsDriveStarted",
                table: "Drives");
        }
    }
}
