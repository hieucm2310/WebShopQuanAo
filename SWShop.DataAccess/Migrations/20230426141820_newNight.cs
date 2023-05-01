using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class newNight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLike",
                table: "Rates");

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ApplicationUserId",
                table: "Likes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ProductId",
                table: "Likes",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.AddColumn<bool>(
                name: "IsLike",
                table: "Rates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2178));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2199));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2202));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 25, 21, 1, 36, 184, DateTimeKind.Local).AddTicks(2204));
        }
    }
}
