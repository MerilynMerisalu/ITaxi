using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class FloorPropertiesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinationFloorNumber",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NeedAssitanceEnteringTheBuilding",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedAssitanceLeavingTheBuilding",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PickupFloorNumber",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationFloorNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NeedAssitanceEnteringTheBuilding",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NeedAssitanceLeavingTheBuilding",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PickupFloorNumber",
                table: "Bookings");
        }
    }
}
