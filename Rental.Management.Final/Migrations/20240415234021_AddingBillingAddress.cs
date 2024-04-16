using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Management.Final.Migrations
{
    public partial class AddingBillingAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "CardExpYear",
                table: "ContractPayments",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CardExpMonth",
                table: "ContractPayments",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "CardExpYear",
                table: "ContractPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<int>(
                name: "CardExpMonth",
                table: "ContractPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2);
        }
    }
}
