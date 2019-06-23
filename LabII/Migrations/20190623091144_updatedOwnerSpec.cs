using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab6.Migrations
{
    public partial class updatedOwnerSpec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_OwnerIDId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "OwnerIDId",
                table: "Expenses",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_OwnerIDId",
                table: "Expenses",
                newName: "IX_Expenses_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_OwnerId",
                table: "Expenses",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_OwnerId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Expenses",
                newName: "OwnerIDId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_OwnerId",
                table: "Expenses",
                newName: "IX_Expenses_OwnerIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_OwnerIDId",
                table: "Expenses",
                column: "OwnerIDId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
