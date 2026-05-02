using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersService.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class AndCustomerIdRemovePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
