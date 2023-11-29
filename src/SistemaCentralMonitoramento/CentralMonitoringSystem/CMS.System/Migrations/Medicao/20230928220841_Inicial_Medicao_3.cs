using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.System.Migrations.Medicao
{
    /// <inheritdoc />
    public partial class Inicial_Medicao_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MEDICOES_TBL",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MEDICOES_TBL",
                table: "MEDICOES_TBL",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MEDICOES_TBL",
                table: "MEDICOES_TBL");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MEDICOES_TBL");
        }
    }
}
