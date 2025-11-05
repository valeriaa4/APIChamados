using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIChamados.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interacoes_Usuarios_IdTecnico",
                table: "Interacoes");

            migrationBuilder.DropIndex(
                name: "IX_Interacoes_IdTecnico",
                table: "Interacoes");

            migrationBuilder.DropColumn(
                name: "IdTecnico",
                table: "Interacoes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTecnico",
                table: "Interacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Interacoes_IdTecnico",
                table: "Interacoes",
                column: "IdTecnico");

            migrationBuilder.AddForeignKey(
                name: "FK_Interacoes_Usuarios_IdTecnico",
                table: "Interacoes",
                column: "IdTecnico",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
