using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThongFastFood_Api.Migrations
{
    /// <inheritdoc />
    public partial class addComboId_OrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Product_Product_Id",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<int>(
                name: "Product_Id",
                table: "OrderDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Combo_Id",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Product_Id",
                table: "Cart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Combo_Id",
                table: "OrderDetail",
                column: "Combo_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Product_Product_Id",
                table: "Cart",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Combo_Combo_Id",
                table: "OrderDetail",
                column: "Combo_Id",
                principalTable: "Combo",
                principalColumn: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Product_Product_Id",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Combo_Combo_Id",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_Combo_Id",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Combo_Id",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<int>(
                name: "Product_Id",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Product_Id",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Product_Product_Id",
                table: "Cart",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
