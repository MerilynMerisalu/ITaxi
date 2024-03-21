using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class ChangedDisabilityTypeNameToBeALangStr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisabilityTypeName",
                table: "DisabilityTypes");

            // Step 1: Add the column, but make it nullable
            migrationBuilder.AddColumn<Guid>(
                name: "DisabilityTypeNameId",
                table: "DisabilityTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.NewGuid());
            
            migrationBuilder.CreateIndex(
                name: "IX_DisabilityTypes_DisabilityTypeNameId",
                table: "DisabilityTypes",
                column: "DisabilityTypeNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisabilityTypes_LangStrings_DisabilityTypeNameId",
                table: "DisabilityTypes",
                column: "DisabilityTypeNameId",
                principalTable: "LangStrings",
                principalColumn: "Id",
                 onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisabilityTypes_LangStrings_DisabilityTypeNameId",
                table: "DisabilityTypes");

            migrationBuilder.DropIndex(
                name: "IX_DisabilityTypes_DisabilityTypeNameId",
                table: "DisabilityTypes");

            migrationBuilder.DropColumn(
                name: "DisabilityTypeNameId",
                table: "DisabilityTypes");

            migrationBuilder.AddColumn<string>(
                name: "DisabilityTypeName",
                table: "DisabilityTypes",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }
    }
}
