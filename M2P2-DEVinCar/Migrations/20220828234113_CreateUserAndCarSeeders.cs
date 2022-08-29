using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M2P2_DEVinCar.Migrations
{
    public partial class CreateUserAndCarSeeders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Name", "SuggestedPrice" },
                values: new object[,]
                {
                    { 1, "Punto", 38000.00m },
                    { 2, "Prisma", 42000.00m },
                    { 3, "Fusca", 10000.00m },
                    { 4, "Kombi", 8000.00m },
                    { 5, "TR-4", 180000.00m },
                    { 6, "Camaro", 308000.00m },
                    { 7, "Toro", 138000.00m },
                    { 8, "Pulse", 88000.00m },
                    { 9, "Nivus", 78000.00m },
                    { 10, "Hilux", 238000.00m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "Email", "Name", "Password" },
                values: new object[,]
                {
                    { 1, new DateTime(1991, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "matheus.gevartosky@senai.com", "Matheus Gevartosky", "123456789" },
                    { 2, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rodrigo.raiche@senai.com", "Rodrigo Raiche", "123456789" },
                    { 3, new DateTime(1993, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.reibnitz@senai.com", "Lucas Reibnitz", "123456789" },
                    { 4, new DateTime(1992, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alessandra.soares@senai.com", "Alessandra Soares", "123456789" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");
        }
    }
}
