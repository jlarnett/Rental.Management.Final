using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Management.Final.Migrations
{
    public partial class fixingMistake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "BillingCity",
                table: "RentalContracts");

            migrationBuilder.DropColumn(
                name: "BillingZipCode",
                table: "RentalContracts");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "ContractPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                table: "ContractPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingZipCode",
                table: "ContractPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "ContractPayments");

            migrationBuilder.DropColumn(
                name: "BillingCity",
                table: "ContractPayments");

            migrationBuilder.DropColumn(
                name: "BillingZipCode",
                table: "ContractPayments");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "RentalContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                table: "RentalContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingZipCode",
                table: "RentalContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
