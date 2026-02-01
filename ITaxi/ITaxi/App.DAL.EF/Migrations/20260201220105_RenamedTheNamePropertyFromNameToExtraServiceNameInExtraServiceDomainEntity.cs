using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class RenamedTheNamePropertyFromNameToExtraServiceNameInExtraServiceDomainEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExtraServices");

            migrationBuilder.AddColumn<Guid>(
                name: "ExtraServiceNameId",
                table: "ExtraServices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ExtraServices_ExtraServiceNameId",
                table: "ExtraServices",
                column: "ExtraServiceNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraServices_LangStrings_ExtraServiceNameId",
                table: "ExtraServices",
                column: "ExtraServiceNameId",
                principalTable: "LangStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraServices_LangStrings_ExtraServiceNameId",
                table: "ExtraServices");

            migrationBuilder.DropIndex(
                name: "IX_ExtraServices_ExtraServiceNameId",
                table: "ExtraServices");

            migrationBuilder.DropColumn(
                name: "ExtraServiceNameId",
                table: "ExtraServices");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExtraServices",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
