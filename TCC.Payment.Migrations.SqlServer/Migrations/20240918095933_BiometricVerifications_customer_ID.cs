using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCC.Payment.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class BiometricVerifications_customer_ID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiometricVerifications_Customers_customer_ID",
                table: "BiometricVerifications");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_ID",
                table: "BiometricVerifications",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_BiometricVerifications_Customers_customer_ID",
                table: "BiometricVerifications",
                column: "customer_ID",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiometricVerifications_Customers_customer_ID",
                table: "BiometricVerifications");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_ID",
                table: "BiometricVerifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BiometricVerifications_Customers_customer_ID",
                table: "BiometricVerifications",
                column: "customer_ID",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
