using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class AddedRidetimesToDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "RideTimes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RideTimes_DriverId",
                table: "RideTimes",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideTimes_Drivers_DriverId",
                table: "RideTimes",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideTimes_Drivers_DriverId",
                table: "RideTimes");

            migrationBuilder.DropIndex(
                name: "IX_RideTimes_DriverId",
                table: "RideTimes");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "RideTimes");
        }
    }
}
