using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPropertDescriptionTypeToLangStrInExtraServiceTypeDomainEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExtraServices");

            migrationBuilder.AddColumn<Guid>(
                name: "DescriptionId",
                table: "ExtraServices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ExtraServices_DescriptionId",
                table: "ExtraServices",
                column: "DescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraServices_LangStrings_DescriptionId",
                table: "ExtraServices",
                column: "DescriptionId",
                principalTable: "LangStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtraServices_LangStrings_DescriptionId",
                table: "ExtraServices");

            migrationBuilder.DropIndex(
                name: "IX_ExtraServices_DescriptionId",
                table: "ExtraServices");

            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "ExtraServices");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExtraServices",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
