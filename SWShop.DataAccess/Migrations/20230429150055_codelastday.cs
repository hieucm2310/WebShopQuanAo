using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class codelastday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sizes");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 22, 0, 55, 437, DateTimeKind.Local).AddTicks(3699));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 22, 0, 55, 437, DateTimeKind.Local).AddTicks(3716));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 22, 0, 55, 437, DateTimeKind.Local).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 22, 0, 55, 437, DateTimeKind.Local).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 22, 0, 55, 437, DateTimeKind.Local).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 22, 0, 55, 437, DateTimeKind.Local).AddTicks(3725));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sizes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 1, 12, 31, 463, DateTimeKind.Local).AddTicks(9774));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 1, 12, 31, 463, DateTimeKind.Local).AddTicks(9791));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 1, 12, 31, 463, DateTimeKind.Local).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 1, 12, 31, 463, DateTimeKind.Local).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 1, 12, 31, 463, DateTimeKind.Local).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 29, 1, 12, 31, 463, DateTimeKind.Local).AddTicks(9800));
        }
    }
}
