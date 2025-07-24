using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceApi.Migrations
{
    public partial class MudançaDeTipoPhoneNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customer",
                type: "VARCHAR",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customer",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR");
        }
    }
}
