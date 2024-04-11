using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Management.Final.Migrations
{
    public partial class updatedRentalPropertyImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PropertyImage",
                table: "RentalProperties",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyImage",
                table: "RentalProperties");
        }
    }
}
