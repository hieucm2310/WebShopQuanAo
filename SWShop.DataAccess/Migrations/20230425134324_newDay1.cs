using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newDay1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityRemain",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5310));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5327));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5330));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5331));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5333));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5336));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "QuantityRemain",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "QuantityRemain" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 41, 31, 248, DateTimeKind.Local).AddTicks(2949), 10.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "QuantityRemain" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 41, 31, 248, DateTimeKind.Local).AddTicks(2964), 10.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "QuantityRemain" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 41, 31, 248, DateTimeKind.Local).AddTicks(2966), 10.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "QuantityRemain" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 41, 31, 248, DateTimeKind.Local).AddTicks(2968), 10.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "QuantityRemain" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 41, 31, 248, DateTimeKind.Local).AddTicks(2970), 10.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "QuantityRemain" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 41, 31, 248, DateTimeKind.Local).AddTicks(2972), 10.0 });
        }
    }
}
