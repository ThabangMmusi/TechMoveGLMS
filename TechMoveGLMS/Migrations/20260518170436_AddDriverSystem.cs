using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechMoveGLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "PdfInvoicePath",
                table: "Invoices");

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EmployeeNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    LicenseType = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LicenseExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DateHired = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProfilePicturePath = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriverSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ShiftType = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PickupLocation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DeliveryLocation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    RouteDescription = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SpecialInstructions = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    EstimatedDistanceKM = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualStartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverSchedules_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverSchedules_ServiceRequests_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2408));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2680));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2686));

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "Id", "Address", "CreatedAt", "DateHired", "Email", "EmployeeNumber", "FullName", "LicenseExpiryDate", "LicenseNumber", "LicenseType", "Phone", "ProfilePicturePath", "Status", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, "123 Main St, City", new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2825), new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.smith@techmove.com", "DRV001", "John Smith", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC123456", 2, "+1-555-0101", null, 0, 8 },
                    { 2, "456 Oak Ave, City", new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2833), new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "mike.johnson@techmove.com", "DRV002", "Mike Johnson", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC789012", 1, "+1-555-0102", null, 0, 5 }
                });

            migrationBuilder.InsertData(
                table: "ServiceRequests",
                columns: new[] { "Id", "AmountUSD", "AmountZAR", "CompletedDate", "ContractId", "Description", "ExchangeRate", "RequestDate", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1500.00m, 29250.00m, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Shipment of 20 containers from NY to London", 19.50m, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Container Shipment - Q1 2026" },
                    { 2, 875.50m, 17072.25m, null, 2, "Time-sensitive documents and small packages", 19.50m, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Express Air Freight" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_Email",
                table: "Drivers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_EmployeeNumber",
                table: "Drivers",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverSchedules_DriverId",
                table: "DriverSchedules",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverSchedules_ServiceRequestId",
                table: "DriverSchedules",
                column: "ServiceRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverSchedules");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DeleteData(
                table: "ServiceRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceRequests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "PdfInvoicePath",
                table: "Invoices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 16, 4, 24, 359, DateTimeKind.Utc).AddTicks(4639));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 16, 4, 24, 359, DateTimeKind.Utc).AddTicks(4644));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 16, 4, 24, 359, DateTimeKind.Utc).AddTicks(4911));

            migrationBuilder.UpdateData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 16, 4, 24, 359, DateTimeKind.Utc).AddTicks(4916));

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "AmountUSD", "AmountZAR", "CreatedAt", "DueDate", "ExchangeRate", "InvoiceDate", "InvoiceNumber", "Notes", "PaidDate", "PdfInvoicePath", "ServiceRequestId", "Status" },
                values: new object[,]
                {
                    { 1, 1500.00m, 29250.00m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 19.50m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-0001", "First invoice for annual shipping agreement", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 2 },
                    { 2, 875.50m, 17072.25m, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19.50m, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-0002", "Express cargo service invoice", null, null, 2, 1 }
                });
        }
    }
}
