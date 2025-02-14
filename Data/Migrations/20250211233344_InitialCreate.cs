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
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
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
                        name: "FK_VehicleBranches_Vehicles_BranchId",
                        column: x => x.BranchId,
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
                    CollectionVehicleBranchID = table.Column<int>(type: "INTEGER", nullable: false),
                    DropoffBranchId = table.Column<int>(type: "INTEGER", nullable: true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: true)
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
                        name: "FK_Bookings_VehicleBranches_CollectionVehicleBranchID",
                        column: x => x.CollectionVehicleBranchID,
                        principalTable: "VehicleBranches",
                        principalColumn: "VehicleBranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "City", "Name" },
                values: new object[,]
                {
                    { 1, "Austin", "Austin Branch" },
                    { 2, "Dallas", "Dallas Branch" },
                    { 3, "Houston", "Houston Branch" }
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
                    { 1, 1, true, 100, 1 },
                    { 2, 2, true, 120, 2 },
                    { 3, 3, true, 90, 3 },
                    { 4, 1, false, 140, 4 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CollectionVehicleBranchID", "CustomerName", "DropoffBranchId", "EndTime", "StartTime", "VehicleId" },
                values: new object[,]
                {
                    { 1, 2, "John Smith", 2, new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 3, "Matthew Johnson", 3, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, 1, "Harry Brown", 1, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CollectionVehicleBranchID",
                table: "Bookings",
                column: "CollectionVehicleBranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DropoffBranchId",
                table: "Bookings",
                column: "DropoffBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBranches_BranchId",
                table: "VehicleBranches",
                column: "BranchId");
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
