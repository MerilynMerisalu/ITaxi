using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class RenamingPhotoColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photos_AppUserId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "Photos",
                newName: "ProfilePhoto");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePhoto",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AppUserId",
                table: "Photos",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photos_AppUserId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfilePhoto",
                table: "Photos",
                newName: "ProfilePicture");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AppUserId",
                table: "Photos",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }
    }
}
