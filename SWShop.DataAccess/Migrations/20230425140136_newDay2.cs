using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newDay2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sizes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "QuantitySold",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2178), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2195), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2197), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2199), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2202), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2204), 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "Sizes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "QuantitySold",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5310), 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5327), 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5330), 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5331), 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5333), 0.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "QuantitySold" },
                values: new object[] { new DateTime(2023, 4, 25, 20, 43, 24, 578, DateTimeKind.Local).AddTicks(5336), 0.0 });
        }
    }
}
