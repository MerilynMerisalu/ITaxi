using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class CountryNavigationPropertyAddedToCounty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counties_LangStrings_CountryNameId",
                table: "Counties");

            migrationBuilder.DropIndex(
                name: "IX_Counties_CountryNameId",
                table: "Counties");

            migrationBuilder.DropColumn(
                name: "CountryNameId",
                table: "Counties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryNameId",
                table: "Counties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Counties_CountryNameId",
                table: "Counties",
                column: "CountryNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counties_LangStrings_CountryNameId",
                table: "Counties",
                column: "CountryNameId",
                principalTable: "LangStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
