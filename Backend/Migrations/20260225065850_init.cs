using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BaseSalaryRange = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Designations_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HRA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Allowances = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "IT", "IT Department", false, "Information Technology" },
                    { 2, "HR", "HR Department", false, "Human Resources" },
                    { 3, "FINANCE", "Finance Department", false, "Finance" },
                    { 4, "SALES", "Sales Department", false, "Sales" },
                    { 5, "OPS", "Operations Department", false, "Operations" }
                });

            migrationBuilder.InsertData(
                table: "Designations",
                columns: new[] { "Id", "BaseSalaryRange", "Code", "DepartmentId", "Description", "IsDeleted", "Title" },
                values: new object[,]
                {
                    { 1, 50000m, "SE", 1, "Software Development Role", false, "Software Engineer" },
                    { 2, 80000m, "SM", 1, "Senior Management Role", false, "Senior Manager" },
                    { 3, 60000m, "HRM", 2, "HR Management Role", false, "HR Manager" },
                    { 4, 70000m, "FM", 3, "Finance Management Role", false, "Finance Manager" },
                    { 5, 55000m, "SET", 4, "Sales Role", false, "Sales Executive" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "DesignationId", "Email", "FirstName", "IsDeleted", "LastName" },
                values: new object[,]
                {
                    { 1, 1, 1, "john.smith@company.com", "John", false, "Smith" },
                    { 2, 2, 3, "jane.doe@company.com", "Jane", false, "Doe" },
                    { 3, 3, 4, "robert.johnson@company.com", "Robert", false, "Johnson" },
                    { 4, 4, 5, "michael.brown@company.com", "Michael", false, "Brown" },
                    { 5, 1, 2, "sarah.wilson@company.com", "Sarah", false, "Wilson" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeSalaries",
                columns: new[] { "Id", "Allowances", "BaseSalary", "DA", "EmployeeId", "HRA", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 2000m, 50000m, 5000m, 1, 10000m, false },
                    { 2, 2500m, 60000m, 6000m, 2, 12000m, false },
                    { 3, 3000m, 70000m, 7000m, 3, 14000m, false },
                    { 4, 2200m, 55000m, 5500m, 4, 11000m, false },
                    { 5, 3500m, 80000m, 8000m, 5, 16000m, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_IsDeleted",
                table: "Departments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Code",
                table: "Designations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Designations_DepartmentId",
                table: "Designations",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_DepartmentId_IsDeleted",
                table: "Designations",
                columns: new[] { "DepartmentId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_IsDeleted",
                table: "Designations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId_IsDeleted",
                table: "Employees",
                columns: new[] { "DepartmentId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DesignationId_IsDeleted",
                table: "Employees",
                columns: new[] { "DesignationId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IsDeleted",
                table: "Employees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EmployeeId_IsDeleted",
                table: "EmployeeSalaries",
                columns: new[] { "EmployeeId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_IsDeleted",
                table: "EmployeeSalaries",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSalaries");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
