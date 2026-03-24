using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGEI.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaConfiguracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracoesClinica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoraAbertura = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFechamento = table.Column<TimeSpan>(type: "time", nullable: false),
                    DiasFuncionamento = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesClinica", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesClinica");
        }
    }
}
