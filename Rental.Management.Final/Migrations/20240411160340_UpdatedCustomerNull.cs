using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Management.Final.Migrations
{
    public partial class UpdatedCustomerNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_RentalProperties_RentalPropertyId",
                table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "RentalPropertyId",
                table: "Customers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_RentalProperties_RentalPropertyId",
                table: "Customers",
                column: "RentalPropertyId",
                principalTable: "RentalProperties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_RentalProperties_RentalPropertyId",
                table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "RentalPropertyId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_RentalProperties_RentalPropertyId",
                table: "Customers",
                column: "RentalPropertyId",
                principalTable: "RentalProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
