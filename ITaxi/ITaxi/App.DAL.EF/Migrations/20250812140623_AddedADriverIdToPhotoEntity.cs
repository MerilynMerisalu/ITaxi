using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddedADriverIdToPhotoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_DriverId",
                table: "Photos",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Drivers_DriverId",
                table: "Photos",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Drivers_DriverId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_DriverId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Photos");
        }
    }
}
