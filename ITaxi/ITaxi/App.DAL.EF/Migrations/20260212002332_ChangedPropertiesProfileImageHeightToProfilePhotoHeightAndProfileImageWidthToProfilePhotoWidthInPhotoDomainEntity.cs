using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPropertiesProfileImageHeightToProfilePhotoHeightAndProfileImageWidthToProfilePhotoWidthInPhotoDomainEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageWidth",
                table: "Photos",
                newName: "ProfilePhotoWidth");

            migrationBuilder.RenameColumn(
                name: "ProfileImageHeight",
                table: "Photos",
                newName: "ProfilePhotoHeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePhotoWidth",
                table: "Photos",
                newName: "ProfileImageWidth");

            migrationBuilder.RenameColumn(
                name: "ProfilePhotoHeight",
                table: "Photos",
                newName: "ProfileImageHeight");
        }
    }
}
