using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class DataOriginAddedPropertyToDomainMetaIdClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "VehicleTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "VehicleModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "VehicleMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "RideTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "ExtraServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Drives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "DriverLicenseCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "DisabilityTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Counties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataOrigin",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "VehicleModels");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "VehicleMarks");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "RideTimes");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "ExtraServices");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Drives");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "DriverLicenseCategories");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "DisabilityTypes");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Counties");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DataOrigin",
                table: "Admins");
        }
    }
}
