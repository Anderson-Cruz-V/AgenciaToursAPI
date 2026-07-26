using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgenciaToursAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarGuiaTransporteMetodoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuiaTuristicoId",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransporteId",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MetodoPagoId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GuiasTuristicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiasTuristicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetodosPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosPago", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transportes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_GuiaTuristicoId",
                table: "Tours",
                column: "GuiaTuristicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TransporteId",
                table: "Tours",
                column: "TransporteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_MetodoPagoId",
                table: "Reservas",
                column: "MetodoPagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_MetodosPago_MetodoPagoId",
                table: "Reservas",
                column: "MetodoPagoId",
                principalTable: "MetodosPago",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_GuiasTuristicos_GuiaTuristicoId",
                table: "Tours",
                column: "GuiaTuristicoId",
                principalTable: "GuiasTuristicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Transportes_TransporteId",
                table: "Tours",
                column: "TransporteId",
                principalTable: "Transportes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_MetodosPago_MetodoPagoId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_GuiasTuristicos_GuiaTuristicoId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Transportes_TransporteId",
                table: "Tours");

            migrationBuilder.DropTable(
                name: "GuiasTuristicos");

            migrationBuilder.DropTable(
                name: "MetodosPago");

            migrationBuilder.DropTable(
                name: "Transportes");

            migrationBuilder.DropIndex(
                name: "IX_Tours_GuiaTuristicoId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TransporteId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_MetodoPagoId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "GuiaTuristicoId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "TransporteId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MetodoPagoId",
                table: "Reservas");
        }
    }
}
