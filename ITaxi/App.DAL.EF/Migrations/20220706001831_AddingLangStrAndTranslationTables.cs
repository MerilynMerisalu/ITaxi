using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class AddingLangStrAndTranslationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleTypeName",
                table: "VehicleTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleTypeNameId",
                table: "VehicleTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LangStrings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangStrings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 10240, nullable: false),
                    LangStrId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translations_LangStrings_LangStrId",
                        column: x => x.LangStrId,
                        principalTable: "LangStrings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_VehicleTypeNameId",
                table: "VehicleTypes",
                column: "VehicleTypeNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LangStrId",
                table: "Translations",
                column: "LangStrId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleTypes_LangStrings_VehicleTypeNameId",
                table: "VehicleTypes",
                column: "VehicleTypeNameId",
                principalTable: "LangStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypes_LangStrings_VehicleTypeNameId",
                table: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "LangStrings");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypes_VehicleTypeNameId",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "VehicleTypeNameId",
                table: "VehicleTypes");

            migrationBuilder.AddColumn<string>(
                name: "VehicleTypeName",
                table: "VehicleTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
