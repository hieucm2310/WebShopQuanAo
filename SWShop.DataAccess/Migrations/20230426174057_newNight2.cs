using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newNight2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "OrderHeaders",
                newName: "Ward");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "OrderHeaders",
                newName: "Province");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DistrictNo",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceNo",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WardNo",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 27, 0, 40, 57, 502, DateTimeKind.Local).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 27, 0, 40, 57, 502, DateTimeKind.Local).AddTicks(4990));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 27, 0, 40, 57, 502, DateTimeKind.Local).AddTicks(4993));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 27, 0, 40, 57, 502, DateTimeKind.Local).AddTicks(4996));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 27, 0, 40, 57, 502, DateTimeKind.Local).AddTicks(5061));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 27, 0, 40, 57, 502, DateTimeKind.Local).AddTicks(5064));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "District",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "DistrictNo",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ProvinceNo",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "WardNo",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "OrderHeaders",
                newName: "StreetAddress");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "OrderHeaders",
                newName: "City");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 22, 36, 38, 97, DateTimeKind.Local).AddTicks(8152));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 22, 36, 38, 97, DateTimeKind.Local).AddTicks(8170));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 22, 36, 38, 97, DateTimeKind.Local).AddTicks(8172));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 22, 36, 38, 97, DateTimeKind.Local).AddTicks(8175));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 22, 36, 38, 97, DateTimeKind.Local).AddTicks(8177));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 22, 36, 38, 97, DateTimeKind.Local).AddTicks(8179));
        }
    }
}
