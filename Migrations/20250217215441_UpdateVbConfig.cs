using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalWebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVbConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "VehicleBranches",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 5,
                column: "IsAvailable",
                value: true);

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 6,
                column: "IsAvailable",
                value: true);

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 7,
                column: "IsAvailable",
                value: true);

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 8,
                column: "IsAvailable",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "VehicleBranches",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 5,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 6,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 7,
                column: "IsAvailable",
                value: false);

            migrationBuilder.UpdateData(
                table: "VehicleBranches",
                keyColumn: "VehicleBranchId",
                keyValue: 8,
                column: "IsAvailable",
                value: false);
        }
    }
}
