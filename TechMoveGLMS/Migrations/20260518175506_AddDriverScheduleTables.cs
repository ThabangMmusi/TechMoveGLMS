using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechMoveGLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverScheduleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverSchedules_ServiceRequests_ServiceRequestId",
                table: "DriverSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_ServiceRequests_ServiceRequestId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Contracts_ContractId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_DriverSchedules_ServiceRequestId",
                table: "DriverSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_Email",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_EmployeeNumber",
                table: "Drivers");

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Drivers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceRequests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ActualEndTime",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "ActualStartTime",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "EstimatedDistanceKM",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "RouteDescription",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "ShiftType",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "SpecialInstructions",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "DriverSchedules");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseExpiryDate",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseType",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Drivers",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "PickupLocation",
                table: "DriverSchedules",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryLocation",
                table: "DriverSchedules",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenseNumber",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_ServiceRequests_ServiceRequestId",
                table: "Invoices",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Contracts_ContractId",
                table: "ServiceRequests",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_ServiceRequests_ServiceRequestId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Contracts_ContractId",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Drivers",
                newName: "FullName");

            migrationBuilder.AlterColumn<string>(
                name: "PickupLocation",
                table: "DriverSchedules",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryLocation",
                table: "DriverSchedules",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndTime",
                table: "DriverSchedules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartTime",
                table: "DriverSchedules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "DriverSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDistanceKM",
                table: "DriverSchedules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RouteDescription",
                table: "DriverSchedules",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceRequestId",
                table: "DriverSchedules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShiftType",
                table: "DriverSchedules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialInstructions",
                table: "DriverSchedules",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "DriverSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<string>(
                name: "LicenseNumber",
                table: "Drivers",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Drivers",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                table: "Drivers",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseExpiryDate",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LicenseType",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "Drivers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "Name", "Phone", "Region" },
                values: new object[,]
                {
                    { 1, "123 Trade Center, New York, NY 10001", new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2403), "contact@globalfreight.com", "Global Freight Solutions", "+1-555-0101", "North America" },
                    { 2, "Marienstraße 15, Berlin 10117", new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2408), "info@eurologistics.de", "EuroLogistics GmbH", "+49-30-1234567", "Europe" }
                });

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "Id", "Address", "CreatedAt", "DateHired", "Email", "EmployeeNumber", "FullName", "LicenseExpiryDate", "LicenseNumber", "LicenseType", "Phone", "ProfilePicturePath", "Status", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, "123 Main St, City", new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2825), new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.smith@techmove.com", "DRV001", "John Smith", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC123456", 2, "+1-555-0101", null, 0, 8 },
                    { 2, "456 Oak Ave, City", new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2833), new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "mike.johnson@techmove.com", "DRV002", "Mike Johnson", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC789012", 1, "+1-555-0102", null, 0, 5 }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ClientId", "CreatedAt", "Description", "EndDate", "ServiceLevel", "SignedAgreementFileName", "SignedAgreementPath", "StartDate", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2680), "Full-service international shipping with priority handling", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Annual Shipping Agreement 2026" },
                    { 2, 2, new DateTime(2026, 5, 18, 17, 4, 34, 833, DateTimeKind.Utc).AddTicks(2686), "Time-sensitive cargo delivery across Europe", new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Express", null, null, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Express Cargo Services" }
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
                name: "IX_DriverSchedules_ServiceRequestId",
                table: "DriverSchedules",
                column: "ServiceRequestId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverSchedules_ServiceRequests_ServiceRequestId",
                table: "DriverSchedules",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_ServiceRequests_ServiceRequestId",
                table: "Invoices",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Contracts_ContractId",
                table: "ServiceRequests",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
