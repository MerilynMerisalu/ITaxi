using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class SecondTimeRenamedAPropertyDoElectricWheelchairFitInCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoElectricWheelchairFitInVehicle",
                table: "Vehicles",
                newName: "DoesElectricWheelchairFitInVehicle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoesElectricWheelchairFitInVehicle",
                table: "Vehicles",
                newName: "DoElectricWheelchairFitInVehicle");
        }
    }
}
