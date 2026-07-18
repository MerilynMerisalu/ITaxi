using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddedAISOAlpha3PropertyToCountryDomainEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISOCode",
                table: "Countries",
                newName: "ISOCodeAlpha2");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_ISOCode",
                table: "Countries",
                newName: "IX_Countries_ISOCodeAlpha2");

            migrationBuilder.AddColumn<string>(
                name: "ISOCodeAlpha3",
                table: "Countries",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISOCodeAlpha3",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "ISOCodeAlpha2",
                table: "Countries",
                newName: "ISOCode");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_ISOCodeAlpha2",
                table: "Countries",
                newName: "IX_Countries_ISOCode");
        }
    }
}
