using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Type_TypeId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Students",
                newName: "GenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_TypeId",
                table: "Students",
                newName: "IX_Students_GenderId");

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Genders_GenderId",
                table: "Students",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Genders_GenderId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.RenameColumn(
                name: "GenderId",
                table: "Students",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_GenderId",
                table: "Students",
                newName: "IX_Students_TypeId");

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Type_TypeId",
                table: "Students",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "Id");
        }
    }
}
