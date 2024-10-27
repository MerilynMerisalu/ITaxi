using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddedCountryPropertyIntoAdminEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Admins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Admins_CountryId",
                table: "Admins",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Countries_CountryId",
                table: "Admins",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Countries_CountryId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Admins_CountryId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Admins");
        }
    }
}
