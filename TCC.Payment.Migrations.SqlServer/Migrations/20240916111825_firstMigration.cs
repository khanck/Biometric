using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCC.Payment.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    businessTypes = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isEmailVerified = table.Column<bool>(type: "bit", nullable: true),
                    isMobileVerified = table.Column<bool>(type: "bit", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    pin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    isEmailVerified = table.Column<bool>(type: "bit", nullable: true),
                    isMobileVerified = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    accountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    iban = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Business_business_ID",
                        column: x => x.business_ID,
                        principalTable: "Business",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Biometrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    biometricType = table.Column<int>(type: "int", nullable: false),
                    abisReferenceID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    biometricData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biometrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biometrics_Customers_customer_ID",
                        column: x => x.customer_ID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BiometricVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    biometricType = table.Column<int>(type: "int", nullable: false),
                    biometricData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    verificationResponse = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    verificationStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiometricVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiometricVerifications_Customers_customer_ID",
                        column: x => x.customer_ID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nameOnCard = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cardType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    cardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    expiryMonth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    expiryYear = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cvv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    isPrimary = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCards_Customers_customer_ID",
                        column: x => x.customer_ID,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    biometricVerification_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    paymentCard_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    billNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    amount = table.Column<double>(type: "float", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    transactionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_account_ID",
                        column: x => x.account_ID,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transactions_BiometricVerifications_biometricVerification_ID",
                        column: x => x.biometricVerification_ID,
                        principalTable: "BiometricVerifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transactions_PaymentCards_paymentCard_ID",
                        column: x => x.paymentCard_ID,
                        principalTable: "PaymentCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_business_ID",
                table: "Accounts",
                column: "business_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Biometrics_customer_ID",
                table: "Biometrics",
                column: "customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BiometricVerifications_customer_ID",
                table: "BiometricVerifications",
                column: "customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCards_customer_ID",
                table: "PaymentCards",
                column: "customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_account_ID",
                table: "Transactions",
                column: "account_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_biometricVerification_ID",
                table: "Transactions",
                column: "biometricVerification_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_paymentCard_ID",
                table: "Transactions",
                column: "paymentCard_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biometrics");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "BiometricVerifications");

            migrationBuilder.DropTable(
                name: "PaymentCards");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
