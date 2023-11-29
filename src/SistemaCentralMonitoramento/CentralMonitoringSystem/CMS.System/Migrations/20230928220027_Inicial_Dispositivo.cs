using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.System.Migrations
{
    /// <inheritdoc />
    public partial class Inicial_Dispositivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DISPOSITIVOS_TBL",
                columns: table => new
                {
                    ID_DEVICE = table.Column<string>(type: "varchar(100)", nullable: false),
                    ID_GROUP = table.Column<int>(type: "int", nullable: false),
                    FREQUENCY = table.Column<int>(type: "int", nullable: false),
                    DT_CADASTRO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DT_ALTERACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COD_USUARIO_ALTERACAO = table.Column<string>(type: "varchar(100)", nullable: false, defaultValue: "SISTEMA"),
                    IND_ATIVO_HMD_AMB = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO_TMP_AMB = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO_HMD_SOL = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPOSITIVOS_TBL", x => x.ID_DEVICE);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DISPOSITIVOS_TBL");
        }
    }
}
