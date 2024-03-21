using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class Country_Name_Translation_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Countries");

            migrationBuilder.AddColumn<Guid>(
                name: "CountryNameId",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryNameId",
                table: "Countries",
                column: "CountryNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_LangStrings_CountryNameId",
                table: "Countries",
                column: "CountryNameId",
                principalTable: "LangStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_LangStrings_CountryNameId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryNameId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryNameId",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Countries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
