using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_AspNetUsers_UserId1",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId1",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Transactions",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserId1",
                table: "Transactions",
                newName: "IX_Transactions_AppUserId");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Budgets",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_UserId1",
                table: "Budgets",
                newName: "IX_Budgets_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_AspNetUsers_AppUserId",
                table: "Budgets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_AppUserId",
                table: "Transactions",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_AspNetUsers_AppUserId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_AppUserId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Transactions",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AppUserId",
                table: "Transactions",
                newName: "IX_Transactions_UserId1");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Budgets",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_AppUserId",
                table: "Budgets",
                newName: "IX_Budgets_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_AspNetUsers_UserId1",
                table: "Budgets",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId1",
                table: "Transactions",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
