using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class RenamedAPropertyDoElectricWheelchairFitInCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoElectricWheelchairFitInCar",
                table: "Vehicles",
                newName: "DoElectricWheelchairFitInVehicle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoElectricWheelchairFitInVehicle",
                table: "Vehicles",
                newName: "DoElectricWheelchairFitInCar");
        }
    }
}
