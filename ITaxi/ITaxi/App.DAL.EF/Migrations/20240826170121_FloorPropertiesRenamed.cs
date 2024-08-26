using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class FloorPropertiesRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NeedAssitanceLeavingTheBuilding",
                table: "Bookings",
                newName: "NeedAssistanceLeavingTheBuilding");

            migrationBuilder.RenameColumn(
                name: "NeedAssitanceEnteringTheBuilding",
                table: "Bookings",
                newName: "NeedAssistanceEnteringTheBuilding");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NeedAssistanceLeavingTheBuilding",
                table: "Bookings",
                newName: "NeedAssitanceLeavingTheBuilding");

            migrationBuilder.RenameColumn(
                name: "NeedAssistanceEnteringTheBuilding",
                table: "Bookings",
                newName: "NeedAssitanceEnteringTheBuilding");
        }
    }
}
