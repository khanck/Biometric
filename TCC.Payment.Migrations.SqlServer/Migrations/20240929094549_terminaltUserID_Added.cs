using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCC.Payment.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class terminaltUserID_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TerminalUserId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 1)
                .Annotation("SqlServer:Identity", "1, 1"); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TerminalUserId",
                table: "Customers");
        }
    }
}
