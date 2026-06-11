using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaEscolar.Web.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarBuzon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Mensajes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Mensajes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Respuesta",
                table: "Mensajes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "Mensajes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_UsuarioIdUsuario",
                table: "Mensajes",
                column: "UsuarioIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensajes_Usuarios_UsuarioIdUsuario",
                table: "Mensajes",
                column: "UsuarioIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensajes_Usuarios_UsuarioIdUsuario",
                table: "Mensajes");

            migrationBuilder.DropIndex(
                name: "IX_Mensajes_UsuarioIdUsuario",
                table: "Mensajes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Mensajes");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Mensajes");

            migrationBuilder.DropColumn(
                name: "Respuesta",
                table: "Mensajes");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "Mensajes");
        }
    }
}
