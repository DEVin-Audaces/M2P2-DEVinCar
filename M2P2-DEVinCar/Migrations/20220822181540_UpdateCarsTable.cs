using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M2P2_DEVinCar.Migrations
{
    public partial class UpdateCarsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrecoSugerido",
                table: "Cars",
                newName: "SuggestedPrice");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Cars",
                newName: "Name");

            migrationBuilder.AlterColumn<decimal>(
                name: "SuggestedPrice",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SuggestedPrice",
                table: "Cars",
                newName: "PrecoSugerido");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cars",
                newName: "Nome");

            migrationBuilder.AlterColumn<double>(
                name: "PrecoSugerido",
                table: "Cars",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
