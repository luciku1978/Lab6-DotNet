using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab6.Migrations
{
    public partial class ChangedOwnerIDInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_OwnerIdId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "OwnerIdId",
                table: "Expenses",
                newName: "OwnerID");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_OwnerIdId",
                table: "Expenses",
                newName: "IX_Expenses_OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_OwnerID",
                table: "Expenses",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_OwnerID",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "OwnerID",
                table: "Expenses",
                newName: "OwnerIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_OwnerID",
                table: "Expenses",
                newName: "IX_Expenses_OwnerIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_OwnerIdId",
                table: "Expenses",
                column: "OwnerIdId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
