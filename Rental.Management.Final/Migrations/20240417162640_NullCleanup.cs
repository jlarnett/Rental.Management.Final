using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Management.Final.Migrations
{
    public partial class NullCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_RentalProperties_RentalPropertyId",
                table: "RentalContracts");

            migrationBuilder.AlterColumn<int>(
                name: "RentalPropertyId",
                table: "RentalContracts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_RentalProperties_RentalPropertyId",
                table: "RentalContracts",
                column: "RentalPropertyId",
                principalTable: "RentalProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalContracts_RentalProperties_RentalPropertyId",
                table: "RentalContracts");

            migrationBuilder.AlterColumn<int>(
                name: "RentalPropertyId",
                table: "RentalContracts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalContracts_RentalProperties_RentalPropertyId",
                table: "RentalContracts",
                column: "RentalPropertyId",
                principalTable: "RentalProperties",
                principalColumn: "Id");
        }
    }
}
