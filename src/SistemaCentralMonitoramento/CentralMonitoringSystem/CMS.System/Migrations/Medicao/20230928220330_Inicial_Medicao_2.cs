using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.System.Migrations.Medicao
{
    /// <inheritdoc />
    public partial class Inicial_Medicao_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MEDICOES_TBL_Dispositivo_DispositivoID_DEVICE",
                table: "MEDICOES_TBL");

            migrationBuilder.DropTable(
                name: "Dispositivo");

            migrationBuilder.DropIndex(
                name: "IX_MEDICOES_TBL_DispositivoID_DEVICE",
                table: "MEDICOES_TBL");

            migrationBuilder.DropColumn(
                name: "DispositivoID_DEVICE",
                table: "MEDICOES_TBL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DispositivoID_DEVICE",
                table: "MEDICOES_TBL",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Dispositivo",
                columns: table => new
                {
                    ID_DEVICE = table.Column<string>(type: "varchar(100)", nullable: false),
                    COD_USUARIO_ALTERACAO = table.Column<string>(type: "varchar(100)", nullable: false),
                    DT_ALTERACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DT_CADASTRO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FREQUENCY = table.Column<int>(type: "int", nullable: false),
                    ID_GROUP = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO_HMD_AMB = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO_HMD_SOL = table.Column<int>(type: "int", nullable: false),
                    IND_ATIVO_TMP_AMB = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivo", x => x.ID_DEVICE);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MEDICOES_TBL_DispositivoID_DEVICE",
                table: "MEDICOES_TBL",
                column: "DispositivoID_DEVICE");

            migrationBuilder.AddForeignKey(
                name: "FK_MEDICOES_TBL_Dispositivo_DispositivoID_DEVICE",
                table: "MEDICOES_TBL",
                column: "DispositivoID_DEVICE",
                principalTable: "Dispositivo",
                principalColumn: "ID_DEVICE",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
