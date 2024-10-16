using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCC.Payment.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Trigger_table_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {      
            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    account_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    billNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    amount = table.Column<double>(type: "float", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Triggers");

            migrationBuilder.AlterColumn<int>(
                name: "TerminalUserId",
                table: "Customers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
