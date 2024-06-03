using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThongFastFood_Api.Migrations
{
    /// <inheritdoc />
    public partial class updateTblCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Cart",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Cart",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<int>(
                name: "Subtotal",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
