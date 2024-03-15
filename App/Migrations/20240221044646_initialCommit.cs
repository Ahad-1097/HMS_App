using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class initialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Dr_ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrId = table.Column<string>(nullable: true),
                    Dr_Name = table.Column<string>(nullable: true),
                    Specialty = table.Column<string>(nullable: true),
                    Education = table.Column<string>(nullable: true),
                    Certifications = table.Column<string>(nullable: true),
                    Experience = table.Column<int>(nullable: false),
                    Schedule = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    ContactInformation = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Dr_ID);
                });

            migrationBuilder.CreateTable(
                name: "InvestigationImages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestigationID = table.Column<long>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    BloodSugar_img = table.Column<string>(nullable: true),
                    TFT_img = table.Column<string>(nullable: true),
                    USG_img = table.Column<string>(nullable: true),
                    SONOMMOGRAPHY_img = table.Column<string>(nullable: true),
                    CECT_img = table.Column<string>(nullable: true),
                    MRI_img = table.Column<string>(nullable: true),
                    FNAC_img = table.Column<string>(nullable: true),
                    TrucutBiopsy_img = table.Column<string>(nullable: true),
                    ReceptorStatus_img = table.Column<string>(nullable: true),
                    MRCP_img = table.Column<string>(nullable: true),
                    ERCP_img = table.Column<string>(nullable: true),
                    EndoscopyUpperGI_img = table.Column<string>(nullable: true),
                    EndoscopyLowerGI_img = table.Column<string>(nullable: true),
                    PETCT_img = table.Column<string>(nullable: true),
                    TumorMarkers_img = table.Column<string>(nullable: true),
                    IVP_img = table.Column<string>(nullable: true),
                    MCU_img = table.Column<string>(nullable: true),
                    RGU_img = table.Column<string>(nullable: true),
                    ABG_img = table.Column<string>(nullable: true),
                    CBC_img = table.Column<string>(nullable: true),
                    RFT_img = table.Column<string>(nullable: true),
                    PTINR_img = table.Column<string>(nullable: true),
                    LFT_img = table.Column<string>(nullable: true),
                    LIPIDPROFILE_img = table.Column<string>(nullable: true),
                    UrineRM_img = table.Column<string>(nullable: true),
                    PTDNR_img = table.Column<string>(nullable: true),
                    OtherO = table.Column<string>(nullable: true),
                    OtherT = table.Column<string>(nullable: true),
                    OtherTh = table.Column<string>(nullable: true),
                    Msg = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outcome",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(nullable: true),
                    PatientID = table.Column<long>(nullable: false),
                    outcomeType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outcome", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrId = table.Column<string>(nullable: true),
                    CADSNumber = table.Column<string>(nullable: true),
                    OPDNumber = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DOA = table.Column<DateTime>(nullable: false),
                    Address_ID = table.Column<long>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    AlternateNumber = table.Column<string>(nullable: true),
                    SeniorResident = table.Column<string>(nullable: true),
                    JuniorResident = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Daignosis = table.Column<string>(nullable: true),
                    Side = table.Column<string>(nullable: true),
                    CoMorbity = table.Column<string>(nullable: true),
                    OtherO = table.Column<string>(nullable: true),
                    OtherT = table.Column<string>(nullable: true),
                    OtherTh = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    SerialNumber = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_Patient_Address_Address_ID",
                        column: x => x.Address_ID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    SubCategoryTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseSheet",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<long>(nullable: false),
                    PRESENTING_COMPLAINTS = table.Column<string>(nullable: true),
                    HistoryOfPresentingIllness = table.Column<string>(nullable: true),
                    PastHistory = table.Column<string>(nullable: true),
                    PersonalHistory = table.Column<string>(nullable: true),
                    Diet = table.Column<string>(nullable: true),
                    Appetite = table.Column<string>(nullable: true),
                    Sleep = table.Column<string>(nullable: true),
                    Bowel = table.Column<string>(nullable: true),
                    Bladder = table.Column<string>(nullable: true),
                    Addiction = table.Column<string>(nullable: true),
                    FamilyHistory = table.Column<string>(nullable: true),
                    BP = table.Column<string>(nullable: true),
                    PR = table.Column<string>(nullable: true),
                    RR = table.Column<string>(nullable: true),
                    Temp = table.Column<string>(nullable: true),
                    SpO2 = table.Column<string>(nullable: true),
                    Pallor = table.Column<string>(nullable: true),
                    Icterus = table.Column<string>(nullable: true),
                    Cyanosis = table.Column<string>(nullable: true),
                    Clubbing = table.Column<string>(nullable: true),
                    Edema = table.Column<string>(nullable: true),
                    Lymphadenopathy = table.Column<string>(nullable: true),
                    RespiratorySystem = table.Column<string>(nullable: true),
                    CNS = table.Column<string>(nullable: true),
                    CVS = table.Column<string>(nullable: true),
                    PerAbdomen = table.Column<string>(nullable: true),
                    LocoregionalExam = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Value1 = table.Column<string>(nullable: true),
                    Value2 = table.Column<string>(nullable: true),
                    Value3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseSheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseSheet_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discharge",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<long>(nullable: false),
                    DOA = table.Column<string>(nullable: true),
                    DOD = table.Column<string>(nullable: true),
                    Diagnosis = table.Column<string>(nullable: true),
                    CaseSummary = table.Column<string>(nullable: true),
                    Investigations = table.Column<string>(nullable: true),
                    TreatmentGiven = table.Column<string>(nullable: true),
                    AdviceOndischarge = table.Column<string>(nullable: true),
                    SeniorResident = table.Column<string>(nullable: true),
                    JuniorResident = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discharge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discharge_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Investigation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<long>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    HB = table.Column<string>(nullable: true),
                    TLC = table.Column<string>(nullable: true),
                    PLT = table.Column<string>(nullable: true),
                    SGeat = table.Column<string>(nullable: true),
                    BUN = table.Column<string>(nullable: true),
                    Fasting = table.Column<string>(nullable: true),
                    PP = table.Column<string>(nullable: true),
                    Random = table.Column<string>(nullable: true),
                    TotalBil = table.Column<string>(nullable: true),
                    DirectBil = table.Column<string>(nullable: true),
                    AlkPhosphate = table.Column<string>(nullable: true),
                    SGDT = table.Column<string>(nullable: true),
                    SGPT = table.Column<string>(nullable: true),
                    T3 = table.Column<string>(nullable: true),
                    T4 = table.Column<string>(nullable: true),
                    TSH = table.Column<string>(nullable: true),
                    FT3 = table.Column<string>(nullable: true),
                    FT4 = table.Column<string>(nullable: true),
                    Sodium = table.Column<string>(nullable: true),
                    Potassium = table.Column<string>(nullable: true),
                    Calcium = table.Column<string>(nullable: true),
                    PT = table.Column<string>(nullable: true),
                    INR = table.Column<string>(nullable: true),
                    Cholesterol = table.Column<string>(nullable: true),
                    Triglyceride = table.Column<string>(nullable: true),
                    HDL = table.Column<string>(nullable: true),
                    LDL = table.Column<string>(nullable: true),
                    Blood = table.Column<string>(nullable: true),
                    PusCell = table.Column<string>(nullable: true),
                    EpithelialCell = table.Column<string>(nullable: true),
                    Crystals = table.Column<string>(nullable: true),
                    Sugar = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Appearance = table.Column<string>(nullable: true),
                    Albumin = table.Column<string>(nullable: true),
                    ABG = table.Column<string>(nullable: true),
                    USG = table.Column<string>(nullable: true),
                    SONOMMOGRAPHY = table.Column<string>(nullable: true),
                    CECT = table.Column<string>(nullable: true),
                    MRI = table.Column<string>(nullable: true),
                    FNAC = table.Column<string>(nullable: true),
                    TrucutBiopsy = table.Column<string>(nullable: true),
                    ReceptorStatus = table.Column<string>(nullable: true),
                    MRCP = table.Column<string>(nullable: true),
                    ERCP = table.Column<string>(nullable: true),
                    EndoscopyUpperGI = table.Column<string>(nullable: true),
                    EndoscopyLowerGI = table.Column<string>(nullable: true),
                    PETCT = table.Column<string>(nullable: true),
                    TumorMarkers = table.Column<string>(nullable: true),
                    IVP = table.Column<string>(nullable: true),
                    MCU = table.Column<string>(nullable: true),
                    RGU = table.Column<string>(nullable: true),
                    OtherO = table.Column<string>(nullable: true),
                    OtherT = table.Column<string>(nullable: true),
                    OtherTh = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investigation_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<long>(nullable: false),
                    Dr_ID = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    Indication = table.Column<string>(nullable: true),
                    Anaesthetist = table.Column<string>(nullable: true),
                    OpertingSurgeon = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Anaesthesia = table.Column<string>(nullable: true),
                    PreoperativeDiagnosis = table.Column<string>(nullable: true),
                    OperationTitle = table.Column<string>(nullable: true),
                    Findings = table.Column<string>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    StepsOfOperation = table.Column<string>(nullable: true),
                    Antibiotics = table.Column<string>(nullable: true),
                    SpecimensSentFor = table.Column<string>(nullable: true),
                    PostOperativeInstructions = table.Column<string>(nullable: true),
                    PerOPImage = table.Column<string>(nullable: true),
                    Value1 = table.Column<string>(nullable: true),
                    Value2 = table.Column<string>(nullable: true),
                    Value3 = table.Column<string>(nullable: true),
                    Value4 = table.Column<string>(nullable: true),
                    Value5 = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operation_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Progress",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<long>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    Cc = table.Column<string>(nullable: true),
                    GeneralCondition = table.Column<string>(nullable: true),
                    Vitals = table.Column<string>(nullable: true),
                    PR = table.Column<string>(nullable: true),
                    BP = table.Column<string>(nullable: true),
                    RR = table.Column<string>(nullable: true),
                    SpO2 = table.Column<string>(nullable: true),
                    Temp = table.Column<string>(nullable: true),
                    GeneralExamination = table.Column<string>(nullable: true),
                    CNS = table.Column<string>(nullable: true),
                    CVS = table.Column<string>(nullable: true),
                    RS = table.Column<string>(nullable: true),
                    PA = table.Column<string>(nullable: true),
                    LocalExamination = table.Column<string>(nullable: true),
                    Drains = table.Column<string>(nullable: true),
                    Urine = table.Column<string>(nullable: true),
                    Advice = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Progress_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSheet_PatientID",
                table: "CaseSheet",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Discharge_PatientID",
                table: "Discharge",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Investigation_PatientID",
                table: "Investigation",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_PatientID",
                table: "Operation",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Address_ID",
                table: "Patient",
                column: "Address_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Progress_PatientID",
                table: "Progress",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_CategoryId",
                table: "SubCategory",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CaseSheet");

            migrationBuilder.DropTable(
                name: "Discharge");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Investigation");

            migrationBuilder.DropTable(
                name: "InvestigationImages");

            migrationBuilder.DropTable(
                name: "Operation");

            migrationBuilder.DropTable(
                name: "Outcome");

            migrationBuilder.DropTable(
                name: "Progress");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
