using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionCompte.Migrations
{
    /// <inheritdoc />
    public partial class Pointage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Pointer",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pointer",
                table: "Transactions");
        }
    }
}
