using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThongFastFood_Api.Migrations
{
    /// <inheritdoc />
    public partial class updataTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Product_Product_Id",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_Order_ID",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "Order_ID",
                table: "OrderDetail",
                newName: "Order_Id");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_Order_ID",
                table: "OrderDetail",
                newName: "IX_OrderDetail_Order_Id");

            migrationBuilder.AlterColumn<int>(
                name: "Product_Id",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Category",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
                name: "FK_OrderDetail_Order_Order_Id",
                table: "OrderDetail",
                column: "Order_Id",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Product_Product_Id",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_Order_Id",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "Order_Id",
                table: "OrderDetail",
                newName: "Order_ID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_Order_Id",
                table: "OrderDetail",
                newName: "IX_OrderDetail_Order_ID");

            migrationBuilder.AlterColumn<int>(
                name: "Product_Id",
                table: "OrderDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "Product_Id",
                table: "Cart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Product_Product_Id",
                table: "Cart",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_Order_ID",
                table: "OrderDetail",
                column: "Order_ID",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_Product_Id",
                table: "OrderDetail",
                column: "Product_Id",
                principalTable: "Product",
                principalColumn: "ProductId");
        }
    }
}
