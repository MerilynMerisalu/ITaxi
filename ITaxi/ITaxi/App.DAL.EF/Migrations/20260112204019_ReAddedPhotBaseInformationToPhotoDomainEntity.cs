using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class ReAddedPhotBaseInformationToPhotoDomainEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OriginalBytes",
                table: "Photos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhotoBytes",
                table: "Photos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ThumbnailBytes",
                table: "Photos",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalBytes",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoBytes",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ThumbnailBytes",
                table: "Photos");
        }
    }
}
