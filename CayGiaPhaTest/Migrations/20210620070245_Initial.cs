using Microsoft.EntityFrameworkCore.Migrations;

namespace CayGiaPhaTest.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    ParentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FatherId = table.Column<int>(type: "int", nullable: true),
                    MotherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parents_Users_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parents_Users_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parents_FatherId",
                table: "Parents",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_MotherId",
                table: "Parents",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ParentsId",
                table: "Users",
                column: "ParentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Parents_ParentsId",
                table: "Users",
                column: "ParentsId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Users_FatherId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Users_MotherId",
                table: "Parents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Parents");
        }
    }
}
