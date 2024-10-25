using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsIgnoredPropertyIntoBaseDomainEntityMetaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "VehicleTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "VehicleModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "VehicleMarks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Translations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "RideTimes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Photos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "LangStrings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Drives",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "DriverLicenseCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "DriverAndDriverLicenseCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "DisabilityTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Counties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Cities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnored",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "VehicleModels");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "VehicleMarks");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "RideTimes");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "LangStrings");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "DriverLicenseCategories");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "DriverAndDriverLicenseCategories");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "DisabilityTypes");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Counties");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsIgnored",
                table: "Admins");
        }
    }
}
