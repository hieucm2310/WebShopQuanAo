using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newNight1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 21, 18, 20, 61, DateTimeKind.Local).AddTicks(564));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 21, 18, 20, 61, DateTimeKind.Local).AddTicks(579));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 21, 18, 20, 61, DateTimeKind.Local).AddTicks(581));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 21, 18, 20, 61, DateTimeKind.Local).AddTicks(583));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 21, 18, 20, 61, DateTimeKind.Local).AddTicks(585));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 26, 21, 18, 20, 61, DateTimeKind.Local).AddTicks(588));
        }
    }
}
