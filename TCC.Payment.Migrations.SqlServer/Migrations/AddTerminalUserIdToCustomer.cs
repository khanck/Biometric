using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Migrations.SqlServer.Migrations
{
    public partial class AddTerminalUserIdToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the new column with auto-increment (IDENTITY)
            migrationBuilder.AddColumn<int>(
                name: "TerminalUserId",
                table: "Customer",
                nullable: false,
                defaultValue: 1)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the column if the migration is rolled back
            migrationBuilder.DropColumn(
                name: "TerminalUserId",
                table: "Customer");
        }
    }
}
