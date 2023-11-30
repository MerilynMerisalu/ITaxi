using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class RideTimeAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "RideTimes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryTime",
                table: "RideTimes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmedBy",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeclinedBy",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideTimes_BookingId",
                table: "RideTimes",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideTimes_Bookings_BookingId",
                table: "RideTimes",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideTimes_Bookings_BookingId",
                table: "RideTimes");

            migrationBuilder.DropIndex(
                name: "IX_RideTimes_BookingId",
                table: "RideTimes");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "RideTimes");

            migrationBuilder.DropColumn(
                name: "ExpiryTime",
                table: "RideTimes");

            migrationBuilder.DropColumn(
                name: "ConfirmedBy",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DeclinedBy",
                table: "Bookings");
        }
    }
}
