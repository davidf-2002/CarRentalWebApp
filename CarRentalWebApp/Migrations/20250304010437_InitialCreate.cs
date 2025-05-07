using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRentalWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Make = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleBranches",
                columns: table => new
                {
                    VehicleBranchId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rate = table.Column<int>(type: "INTEGER", nullable: false),
                    BranchId = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleBranches", x => x.VehicleBranchId);
                    table.ForeignKey(
                        name: "FK_VehicleBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleBranches_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    PickupBranchId = table.Column<int>(type: "INTEGER", nullable: false),
                    DropoffBranchId = table.Column<int>(type: "INTEGER", nullable: true),
                    VehicleBranchId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Branches_DropoffBranchId",
                        column: x => x.DropoffBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_Bookings_Branches_PickupBranchId",
                        column: x => x.PickupBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_VehicleBranches_VehicleBranchId",
                        column: x => x.VehicleBranchId,
                        principalTable: "VehicleBranches",
                        principalColumn: "VehicleBranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "City", "Name" },
                values: new object[,]
                {
                    { 1, "Austin", "Austin Branch" },
                    { 2, "Dallas", "Dallas Branch" },
                    { 3, "Houston", "Houston Branch" },
                    { 4, "Phoenix", "Phoenix Branch" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleId", "Make", "Model", "Type", "Year" },
                values: new object[,]
                {
                    { 1, "BMW", "320d", "Sedan", 2017 },
                    { 2, "Audi", "A4", "Sedan", 2018 },
                    { 3, "Tesla", "Model S", "Sedan", 2020 },
                    { 4, "Toyota", "Prius", "Hatchback", 2019 }
                });

            migrationBuilder.InsertData(
                table: "VehicleBranches",
                columns: new[] { "VehicleBranchId", "BranchId", "IsAvailable", "Rate", "VehicleId" },
                values: new object[,]
                {
                    { 1, 4, true, 110, 1 },
                    { 2, 3, true, 125, 2 },
                    { 3, 2, true, 95, 3 },
                    { 4, 1, true, 135, 4 }
                });

            migrationBuilder.InsertData(
                table: "VehicleBranches",
                columns: new[] { "VehicleBranchId", "BranchId", "Rate", "VehicleId" },
                values: new object[,]
                {
                    { 5, 1, 105, 1 },
                    { 6, 2, 88, 2 },
                    { 7, 3, 120, 3 },
                    { 8, 4, 130, 4 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CustomerName", "DropoffBranchId", "EndTime", "PickupBranchId", "StartTime", "VehicleBranchId", "VehicleId" },
                values: new object[,]
                {
                    { 1, "John Smith", 4, new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, "Matthew Johnson", 3, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 3, "Harry Brown", 2, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3 },
                    { 4, "Paul Johnson", 1, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DropoffBranchId",
                table: "Bookings",
                column: "DropoffBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PickupBranchId",
                table: "Bookings",
                column: "PickupBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleBranchId",
                table: "Bookings",
                column: "VehicleBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBranches_BranchId",
                table: "VehicleBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBranches_VehicleId",
                table: "VehicleBranches",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "VehicleBranches");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
