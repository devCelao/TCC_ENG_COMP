using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.System.Migrations.Medicao
{
    /// <inheritdoc />
    public partial class Inicial_Medicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dispositivo",
                columns: table => new
                {
                    ID_DEVICE = table.Column<string>(type: "varchar(100)", nullable: false),
                    ID_GROUP = table.Column<int>(type: "int", nullable: false),
                    FREQUENCY = table.Column<int>(type: "int", nullable: false),
                    DT_CADASTRO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DT_ALTERACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COD_USUARIO_ALTERACAO = table.Column<string>(type: "varchar(100)", nullable: false),
                    IND_ATIVO_HMD_AMB = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO_TMP_AMB = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO_HMD_SOL = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivo", x => x.ID_DEVICE);
                });

            migrationBuilder.CreateTable(
                name: "MEDICOES_TBL",
                columns: table => new
                {
                    ID_DEVICE = table.Column<string>(type: "varchar(100)", nullable: false),
                    DT_PUBLICACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VAL_HMD_AMB = table.Column<float>(type: "real", nullable: false),
                    VAL_TMP_AMB = table.Column<float>(type: "real", nullable: false),
                    VAL_HMD_SOL = table.Column<float>(type: "real", nullable: false),
                    DispositivoID_DEVICE = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_MEDICOES_TBL_Dispositivo_DispositivoID_DEVICE",
                        column: x => x.DispositivoID_DEVICE,
                        principalTable: "Dispositivo",
                        principalColumn: "ID_DEVICE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MEDICOES_TBL_DispositivoID_DEVICE",
                table: "MEDICOES_TBL",
                column: "DispositivoID_DEVICE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MEDICOES_TBL");

            migrationBuilder.DropTable(
                name: "Dispositivo");
        }
    }
}
