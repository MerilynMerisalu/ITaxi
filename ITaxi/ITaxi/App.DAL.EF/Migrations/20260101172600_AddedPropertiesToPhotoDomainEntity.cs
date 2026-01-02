using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertiesToPhotoDomainEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Photos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OriginalPhotoHeight",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginalPhotoWidth",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhotoHeight",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhotoWidth",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileImageHeight",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "OriginalPhotoHeight",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "OriginalPhotoWidth",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoHeight",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoWidth",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ProfileImageHeight",
                table: "Photos");
        }
    }
}
