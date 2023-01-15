using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class FPTVMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationChanges_UserAccount_userAccountID",
                table: "AuthenticationChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationLog_UserAccount_userAccountID",
                table: "AuthenticationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationRecovery_UserAccount_userAccountID",
                table: "AuthenticationRecovery");

            migrationBuilder.DropForeignKey(
                name: "FK_Mail_UserAccount_userAccountID",
                table: "Mail");

            migrationBuilder.DropForeignKey(
                name: "FK_Token_UserAccount_userAccountID",
                table: "Token");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_Profile_userId",
                table: "UserAccount");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a432a5d-8ef4-4619-8d70-a025ab9a4ab4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ba145cf-2767-4e5b-866c-b47e992fdcda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92d9c8d8-81df-40f2-bec1-8fae6af1d5e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "944e7d71-5b3d-4044-97a4-053a5c162335");

            migrationBuilder.RenameColumn(
                name: "validated",
                table: "UserAccount",
                newName: "Validated");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "UserAccount",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "UserAccount",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "UserAccount",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "authenticationType",
                table: "UserAccount",
                newName: "AuthenticationType");

            migrationBuilder.RenameColumn(
                name: "userAccountId",
                table: "UserAccount",
                newName: "UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_userId",
                table: "UserAccount",
                newName: "IX_UserAccount_UserId");

            migrationBuilder.RenameColumn(
                name: "userAccountID",
                table: "Token",
                newName: "UserAccountId");

            migrationBuilder.RenameColumn(
                name: "startTime",
                table: "Token",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "endTime",
                table: "Token",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Token",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "tokenId",
                table: "Token",
                newName: "TokenId");

            migrationBuilder.RenameIndex(
                name: "IX_Token_userAccountID",
                table: "Token",
                newName: "IX_Token_UserAccountId");

            migrationBuilder.RenameColumn(
                name: "userAccountID",
                table: "Mail",
                newName: "UserAccountId");

            migrationBuilder.RenameColumn(
                name: "sentDate",
                table: "Mail",
                newName: "SentDate");

            migrationBuilder.RenameColumn(
                name: "senderMail",
                table: "Mail",
                newName: "SenderMail");

            migrationBuilder.RenameColumn(
                name: "receiverMail",
                table: "Mail",
                newName: "ReceiverMail");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Mail",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "mailId",
                table: "Mail",
                newName: "MailId");

            migrationBuilder.RenameIndex(
                name: "IX_Mail_userAccountID",
                table: "Mail",
                newName: "IX_Mail_UserAccountId");

            migrationBuilder.RenameColumn(
                name: "userAccountID",
                table: "AuthenticationRecovery",
                newName: "UserAccountId");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "AuthenticationRecovery",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "newPassword",
                table: "AuthenticationRecovery",
                newName: "NewPassword");

            migrationBuilder.RenameColumn(
                name: "confirmNewPassword",
                table: "AuthenticationRecovery",
                newName: "ConfirmNewPassword");

            migrationBuilder.RenameIndex(
                name: "IX_AuthenticationRecovery_userAccountID",
                table: "AuthenticationRecovery",
                newName: "IX_AuthenticationRecovery_UserAccountId");

            migrationBuilder.RenameColumn(
                name: "userAccountID",
                table: "AuthenticationLog",
                newName: "UserAccountId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "AuthenticationLog",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "authenticationType",
                table: "AuthenticationLog",
                newName: "AuthenticationType");

            migrationBuilder.RenameIndex(
                name: "IX_AuthenticationLog_userAccountID",
                table: "AuthenticationLog",
                newName: "IX_AuthenticationLog_UserAccountId");

            migrationBuilder.RenameColumn(
                name: "userAccountID",
                table: "AuthenticationChanges",
                newName: "UserAccountId");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "AuthenticationChanges",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "AuthenticationChanges",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AuthenticationChanges",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "changed",
                table: "AuthenticationChanges",
                newName: "Changed");

            migrationBuilder.RenameIndex(
                name: "IX_AuthenticationChanges_userAccountID",
                table: "AuthenticationChanges",
                newName: "IX_AuthenticationChanges_UserAccountId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "54ba5474-e57a-4241-97f0-794f495fa616", "01ea0af0-c91c-4a77-a02e-4647a3096344", "moderator", "MODERATOR" },
                    { "9ebb7fe1-6997-424e-8a24-e5b079266bdb", "3c1ec44b-34a2-4a8e-b04f-7e781f4accb0", "user", "USER" },
                    { "9ff60f69-66c2-486a-952c-bcfe51de9476", "6fe81f37-6a1b-4f93-8c3c-289cc7eb77bc", "admin", "ADMIN" },
                    { "d9e32171-1037-41ba-8a53-228ac3174131", "0a8c7a02-efa8-4e39-a7b9-59685ee39fa3", "guest", "GUEST" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationChanges_UserAccount_UserAccountId",
                table: "AuthenticationChanges",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "UserAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationLog_UserAccount_UserAccountId",
                table: "AuthenticationLog",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "UserAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationRecovery_UserAccount_UserAccountId",
                table: "AuthenticationRecovery",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "UserAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mail_UserAccount_UserAccountId",
                table: "Mail",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "UserAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Token_UserAccount_UserAccountId",
                table: "Token",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "UserAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_Profile_UserId",
                table: "UserAccount",
                column: "UserId",
                principalTable: "Profile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationChanges_UserAccount_UserAccountId",
                table: "AuthenticationChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationLog_UserAccount_UserAccountId",
                table: "AuthenticationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthenticationRecovery_UserAccount_UserAccountId",
                table: "AuthenticationRecovery");

            migrationBuilder.DropForeignKey(
                name: "FK_Mail_UserAccount_UserAccountId",
                table: "Mail");

            migrationBuilder.DropForeignKey(
                name: "FK_Token_UserAccount_UserAccountId",
                table: "Token");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_Profile_UserId",
                table: "UserAccount");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54ba5474-e57a-4241-97f0-794f495fa616");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ebb7fe1-6997-424e-8a24-e5b079266bdb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ff60f69-66c2-486a-952c-bcfe51de9476");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9e32171-1037-41ba-8a53-228ac3174131");

            migrationBuilder.RenameColumn(
                name: "Validated",
                table: "UserAccount",
                newName: "validated");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserAccount",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UserAccount",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "UserAccount",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "AuthenticationType",
                table: "UserAccount",
                newName: "authenticationType");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "UserAccount",
                newName: "userAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_UserId",
                table: "UserAccount",
                newName: "IX_UserAccount_userId");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "Token",
                newName: "userAccountID");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Token",
                newName: "startTime");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Token",
                newName: "endTime");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Token",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "TokenId",
                table: "Token",
                newName: "tokenId");

            migrationBuilder.RenameIndex(
                name: "IX_Token_UserAccountId",
                table: "Token",
                newName: "IX_Token_userAccountID");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "Mail",
                newName: "userAccountID");

            migrationBuilder.RenameColumn(
                name: "SentDate",
                table: "Mail",
                newName: "sentDate");

            migrationBuilder.RenameColumn(
                name: "SenderMail",
                table: "Mail",
                newName: "senderMail");

            migrationBuilder.RenameColumn(
                name: "ReceiverMail",
                table: "Mail",
                newName: "receiverMail");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Mail",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "MailId",
                table: "Mail",
                newName: "mailId");

            migrationBuilder.RenameIndex(
                name: "IX_Mail_UserAccountId",
                table: "Mail",
                newName: "IX_Mail_userAccountID");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "AuthenticationRecovery",
                newName: "userAccountID");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AuthenticationRecovery",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "NewPassword",
                table: "AuthenticationRecovery",
                newName: "newPassword");

            migrationBuilder.RenameColumn(
                name: "ConfirmNewPassword",
                table: "AuthenticationRecovery",
                newName: "confirmNewPassword");

            migrationBuilder.RenameIndex(
                name: "IX_AuthenticationRecovery_UserAccountId",
                table: "AuthenticationRecovery",
                newName: "IX_AuthenticationRecovery_userAccountID");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "AuthenticationLog",
                newName: "userAccountID");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "AuthenticationLog",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "AuthenticationType",
                table: "AuthenticationLog",
                newName: "authenticationType");

            migrationBuilder.RenameIndex(
                name: "IX_AuthenticationLog_UserAccountId",
                table: "AuthenticationLog",
                newName: "IX_AuthenticationLog_userAccountID");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "AuthenticationChanges",
                newName: "userAccountID");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AuthenticationChanges",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "AuthenticationChanges",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AuthenticationChanges",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Changed",
                table: "AuthenticationChanges",
                newName: "changed");

            migrationBuilder.RenameIndex(
                name: "IX_AuthenticationChanges_UserAccountId",
                table: "AuthenticationChanges",
                newName: "IX_AuthenticationChanges_userAccountID");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a432a5d-8ef4-4619-8d70-a025ab9a4ab4", "eb9c4781-4f6f-44be-bf6b-9ebf81b6b66a", "guest", "GUEST" },
                    { "4ba145cf-2767-4e5b-866c-b47e992fdcda", "6d69e2d7-55ec-4641-a7d4-aa262377f62a", "moderator", "MODERATOR" },
                    { "92d9c8d8-81df-40f2-bec1-8fae6af1d5e0", "c75a78e2-941a-4ffb-85a8-217e38c424d5", "user", "USER" },
                    { "944e7d71-5b3d-4044-97a4-053a5c162335", "00182970-a98d-48a6-88eb-f4fba42b096c", "admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationChanges_UserAccount_userAccountID",
                table: "AuthenticationChanges",
                column: "userAccountID",
                principalTable: "UserAccount",
                principalColumn: "userAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationLog_UserAccount_userAccountID",
                table: "AuthenticationLog",
                column: "userAccountID",
                principalTable: "UserAccount",
                principalColumn: "userAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthenticationRecovery_UserAccount_userAccountID",
                table: "AuthenticationRecovery",
                column: "userAccountID",
                principalTable: "UserAccount",
                principalColumn: "userAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mail_UserAccount_userAccountID",
                table: "Mail",
                column: "userAccountID",
                principalTable: "UserAccount",
                principalColumn: "userAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Token_UserAccount_userAccountID",
                table: "Token",
                column: "userAccountID",
                principalTable: "UserAccount",
                principalColumn: "userAccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_Profile_userId",
                table: "UserAccount",
                column: "userId",
                principalTable: "Profile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
