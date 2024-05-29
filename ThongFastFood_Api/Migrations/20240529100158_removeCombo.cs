using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThongFastFood_Api.Migrations
{
    /// <inheritdoc />
    public partial class removeCombo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Combo_Combo_Id",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Combo_Combo_Id",
                table: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ComboItem");

            migrationBuilder.DropTable(
                name: "Combo");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_Combo_Id",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_Cart_Combo_Id",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Combo_Id",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Combo_Id",
                table: "Cart");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Combo_Id",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Category",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Combo_Id",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Combo",
                columns: table => new
                {
                    ComboId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComboImage = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    ComboName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComboPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combo", x => x.ComboId);
                });

            migrationBuilder.CreateTable(
                name: "ComboItem",
                columns: table => new
                {
                    Combo_Id = table.Column<int>(type: "int", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboItem", x => new { x.Combo_Id, x.Product_Id });
                    table.ForeignKey(
                        name: "FK_ComboItem_Combo_Combo_Id",
                        column: x => x.Combo_Id,
                        principalTable: "Combo",
                        principalColumn: "ComboId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboItem_Product_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Combo_Id",
                table: "OrderDetail",
                column: "Combo_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_Combo_Id",
                table: "Cart",
                column: "Combo_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ComboItem_Product_Id",
                table: "ComboItem",
                column: "Product_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Combo_Combo_Id",
                table: "Cart",
                column: "Combo_Id",
                principalTable: "Combo",
                principalColumn: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Combo_Combo_Id",
                table: "OrderDetail",
                column: "Combo_Id",
                principalTable: "Combo",
                principalColumn: "ComboId");
        }
    }
}
