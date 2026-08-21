using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    PhoneCountryCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PhoneNationalNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxCountryCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TaxCountryCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    PhoneCountryCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PhoneNationalNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    PhoneCountryCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PhoneNationalNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalSchedules",
                columns: table => new
                {
                    ProfessionalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalSchedules", x => x.ProfessionalId);
                    table.ForeignKey(
                        name: "FK_ProfessionalSchedules_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentRegistries",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentRegistries", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_AssignmentRegistries_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Staff_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreCalendars",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCalendars", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_StoreCalendars_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreCatalogs",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCatalogs", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_StoreCatalogs_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    OfferingId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CanceledAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffCalendars",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCalendars", x => new { x.ProfessionalId, x.StoreId });
                    table.ForeignKey(
                        name: "FK_StaffCalendars_ProfessionalSchedules_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "ProfessionalSchedules",
                        principalColumn: "ProfessionalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffCalendars_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    OfferingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => new { x.StoreId, x.ProfessionalId, x.OfferingId });
                    table.ForeignKey(
                        name: "FK_Assignments_AssignmentRegistries_StoreId",
                        column: x => x.StoreId,
                        principalTable: "AssignmentRegistries",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffMembers",
                columns: table => new
                {
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => new { x.StoreId, x.ProfessionalId });
                    table.ForeignKey(
                        name: "FK_StaffMembers_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Staff_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Staff",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreCalendarExceptions",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCalendarExceptions", x => new { x.StoreId, x.Date });
                    table.ForeignKey(
                        name: "FK_StoreCalendarExceptions_StoreCalendars_StoreId",
                        column: x => x.StoreId,
                        principalTable: "StoreCalendars",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreCalendarWorkingDays",
                columns: table => new
                {
                    Day = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCalendarWorkingDays", x => new { x.StoreId, x.Day });
                    table.ForeignKey(
                        name: "FK_StoreCalendarWorkingDays_StoreCalendars_StoreId",
                        column: x => x.StoreId,
                        principalTable: "StoreCalendars",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offerings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offerings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offerings_StoreCatalogs_StoreId",
                        column: x => x.StoreId,
                        principalTable: "StoreCatalogs",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffCalendarExceptions",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCalendarExceptions", x => new { x.ProfessionalId, x.StoreId, x.Date });
                    table.ForeignKey(
                        name: "FK_StaffCalendarExceptions_StaffCalendars_ProfessionalId_StoreId",
                        columns: x => new { x.ProfessionalId, x.StoreId },
                        principalTable: "StaffCalendars",
                        principalColumns: new[] { "ProfessionalId", "StoreId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffCalendarWorkingDays",
                columns: table => new
                {
                    Day = table.Column<int>(type: "int", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCalendarWorkingDays", x => new { x.ProfessionalId, x.StoreId, x.Day });
                    table.ForeignKey(
                        name: "FK_StaffCalendarWorkingDays_StaffCalendars_ProfessionalId_StoreId",
                        columns: x => new { x.ProfessionalId, x.StoreId },
                        principalTable: "StaffCalendars",
                        principalColumns: new[] { "ProfessionalId", "StoreId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffMemberRoles",
                columns: table => new
                {
                    Role = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMemberRoles", x => new { x.StoreId, x.ProfessionalId, x.Role });
                    table.ForeignKey(
                        name: "FK_StaffMemberRoles_StaffMembers_StoreId_ProfessionalId",
                        columns: x => new { x.StoreId, x.ProfessionalId },
                        principalTable: "StaffMembers",
                        principalColumns: new[] { "StoreId", "ProfessionalId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreExceptionTimeRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreExceptionTimeRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreExceptionTimeRanges_StoreCalendarExceptions_StoreId_Date",
                        columns: x => new { x.StoreId, x.Date },
                        principalTable: "StoreCalendarExceptions",
                        principalColumns: new[] { "StoreId", "Date" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreCalendarWorkingDayTimeRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCalendarWorkingDayTimeRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCalendarWorkingDayTimeRanges_StoreCalendarWorkingDays_StoreId_Day",
                        columns: x => new { x.StoreId, x.Day },
                        principalTable: "StoreCalendarWorkingDays",
                        principalColumns: new[] { "StoreId", "Day" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffCalendarExceptionTimeRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCalendarExceptionTimeRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffCalendarExceptionTimeRanges_StaffCalendarExceptions_ProfessionalId_StoreId_Date",
                        columns: x => new { x.ProfessionalId, x.StoreId, x.Date },
                        principalTable: "StaffCalendarExceptions",
                        principalColumns: new[] { "ProfessionalId", "StoreId", "Date" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffCalendarWorkingDayTimeRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffCalendarWorkingDayTimeRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffCalendarWorkingDayTimeRanges_StaffCalendarWorkingDays_ProfessionalId_StoreId_Day",
                        columns: x => new { x.ProfessionalId, x.StoreId, x.Day },
                        principalTable: "StaffCalendarWorkingDays",
                        principalColumns: new[] { "ProfessionalId", "StoreId", "Day" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ProfessionalId",
                table: "Appointments",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StartAt",
                table: "Appointments",
                column: "StartAt");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StoreId",
                table: "Appointments",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ProfessionalId",
                table: "Assignments",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Offerings_StoreId",
                table: "Offerings",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_Email",
                table: "Professionals",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_PhoneCountryCode_PhoneNationalNumber",
                table: "Professionals",
                columns: new[] { "PhoneCountryCode", "PhoneNationalNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_TaxCountryCode_TaxIdNumber",
                table: "Professionals",
                columns: new[] { "TaxCountryCode", "TaxIdNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffCalendarExceptionTimeRanges_ProfessionalId_StoreId_Date",
                table: "StaffCalendarExceptionTimeRanges",
                columns: new[] { "ProfessionalId", "StoreId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_StaffCalendars_StoreId",
                table: "StaffCalendars",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffCalendarWorkingDayTimeRanges_ProfessionalId_StoreId_Day",
                table: "StaffCalendarWorkingDayTimeRanges",
                columns: new[] { "ProfessionalId", "StoreId", "Day" });

            migrationBuilder.CreateIndex(
                name: "IX_StaffMembers_ProfessionalId",
                table: "StaffMembers",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCalendarWorkingDayTimeRanges_StoreId_Day",
                table: "StoreCalendarWorkingDayTimeRanges",
                columns: new[] { "StoreId", "Day" });

            migrationBuilder.CreateIndex(
                name: "IX_StoreExceptionTimeRanges_StoreId_Date",
                table: "StoreExceptionTimeRanges",
                columns: new[] { "StoreId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Email",
                table: "Stores",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_PhoneCountryCode_PhoneNationalNumber",
                table: "Stores",
                columns: new[] { "PhoneCountryCode", "PhoneNationalNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_TaxCountryCode_TaxIdNumber",
                table: "Stores",
                columns: new[] { "TaxCountryCode", "TaxIdNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneCountryCode_PhoneNationalNumber",
                table: "Users",
                columns: new[] { "PhoneCountryCode", "PhoneNationalNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Offerings");

            migrationBuilder.DropTable(
                name: "StaffCalendarExceptionTimeRanges");

            migrationBuilder.DropTable(
                name: "StaffCalendarWorkingDayTimeRanges");

            migrationBuilder.DropTable(
                name: "StaffMemberRoles");

            migrationBuilder.DropTable(
                name: "StoreCalendarWorkingDayTimeRanges");

            migrationBuilder.DropTable(
                name: "StoreExceptionTimeRanges");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AssignmentRegistries");

            migrationBuilder.DropTable(
                name: "StoreCatalogs");

            migrationBuilder.DropTable(
                name: "StaffCalendarExceptions");

            migrationBuilder.DropTable(
                name: "StaffCalendarWorkingDays");

            migrationBuilder.DropTable(
                name: "StaffMembers");

            migrationBuilder.DropTable(
                name: "StoreCalendarWorkingDays");

            migrationBuilder.DropTable(
                name: "StoreCalendarExceptions");

            migrationBuilder.DropTable(
                name: "StaffCalendars");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "StoreCalendars");

            migrationBuilder.DropTable(
                name: "ProfessionalSchedules");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Professionals");
        }
    }
}
