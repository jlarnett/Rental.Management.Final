using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Management.Final.Migrations
{
    public partial class changingRentalProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "RentalProperties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "RentalProperties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
