using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M2P2_DEVinCar.Migrations
{
    public partial class CreateTableStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Initials = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Initials", "Name" },
                values: new object[,]
                {
                    { 1, "AC", "Acre" },
                    { 2, "AL", "Alagoas" },
                    { 3, "AP", "Amapá" },
                    { 4, "AM", "Amazonas" },
                    { 5, "BA", "Bahia" },
                    { 6, "CE", "Ceará" },
                    { 7, "DF", "Distrito Federal" },
                    { 8, "ES", "Espírito Santo" },
                    { 9, "GO", "Goiás" },
                    { 10, "MA", "Maranhão" },
                    { 11, "MT", "Mato Grosso" },
                    { 12, "MS", "Mato Grosso do Sul" },
                    { 13, "MG", "Minas Gerais" },
                    { 14, "PA", "Pará" },
                    { 15, "PB", "Paraíba" },
                    { 16, "PR", "Paraná" },
                    { 17, "PE", "Pernambuco" },
                    { 18, "PI", "Piauí" },
                    { 19, "RJ", "Rio de Janeiro" },
                    { 20, "RN", "Rio Grande do Norte" },
                    { 21, "RS", "Rio Grande do Sul" },
                    { 22, "RO", "Rondônia" },
                    { 23, "RR", "Roraima" },
                    { 24, "SC", "Santa Catarina" },
                    { 25, "SP", "São Paulo" },
                    { 26, "SE", "Sergipe" },
                    { 27, "TO", "Tocantins" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
