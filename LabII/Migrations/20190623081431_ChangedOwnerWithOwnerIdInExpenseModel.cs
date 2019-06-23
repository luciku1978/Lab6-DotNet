using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab6.Migrations
{
    public partial class ChangedOwnerWithOwnerIdInExpenseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_OwnerId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Expenses",
                newName: "OwnerIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_OwnerId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_OwnerIdId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "OwnerIdId",
                table: "Expenses",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_OwnerIdId",
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
    }
}
