using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VacApp_Bovinova_Platform.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdminTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    createdat = table.Column<DateTimeOffset>(name: "created-at", type: "datetime", nullable: true),
                    updatedat = table.Column<DateTimeOffset>(name: "updated-at", type: "datetime", nullable: true),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    emailconfirmed = table.Column<bool>(name: "email-confirmed", type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_admins", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "credentials",
                columns: table => new
                {
                    userid = table.Column<Guid>(name: "user-id", type: "char(36)", nullable: false),
                    accesstoken = table.Column<string>(name: "access-token", type: "longtext", nullable: false),
                    refreshtoken = table.Column<string>(name: "refresh-token", type: "longtext", nullable: false),
                    expiresinseconds = table.Column<long>(name: "expires-in-seconds", type: "bigint", nullable: true),
                    idtoken = table.Column<string>(name: "id-token", type: "longtext", nullable: false),
                    issuedutc = table.Column<DateTime>(name: "issued-utc", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_credentials", x => x.userid);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "microsoft-credentials",
                columns: table => new
                {
                    userid = table.Column<Guid>(name: "user-id", type: "char(36)", nullable: false),
                    accesstoken = table.Column<string>(name: "access-token", type: "longtext", nullable: false),
                    refreshtoken = table.Column<string>(name: "refresh-token", type: "longtext", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    issuedutc = table.Column<DateTime>(name: "issued-utc", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_microsoft-credentials", x => x.userid);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stables",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    limit = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_stables", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    createdat = table.Column<DateTimeOffset>(name: "created-at", type: "datetime", nullable: true),
                    updatedat = table.Column<DateTimeOffset>(name: "updated-at", type: "datetime", nullable: true),
                    username = table.Column<string>(type: "longtext", nullable: false),
                    password = table.Column<string>(type: "longtext", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    emailconfirmed = table.Column<bool>(name: "email-confirmed", type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_users", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bovines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    birthdate = table.Column<DateTime>(name: "birth-date", type: "datetime(6)", nullable: false),
                    breed = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    stableid = table.Column<int>(name: "stable-id", type: "int", nullable: false),
                    bovineimg = table.Column<string>(name: "bovine-img", type: "varchar(300)", maxLength: 300, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_bovines", x => x.id);
                    table.ForeignKey(
                        name: "f-k_bovines_-stable_stable-id",
                        column: x => x.stableid,
                        principalTable: "stables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "campaigns",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    startdate = table.Column<DateTime>(name: "start-date", type: "datetime(6)", nullable: false),
                    enddate = table.Column<DateTime>(name: "end-date", type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    goalid = table.Column<int>(name: "goal-id", type: "int", nullable: false),
                    stableid = table.Column<int>(name: "stable-id", type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_campaigns", x => x.id);
                    table.ForeignKey(
                        name: "f-k_campaigns_-stable_stable-id",
                        column: x => x.stableid,
                        principalTable: "stables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vaccines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    vaccinetype = table.Column<string>(name: "vaccine-type", type: "varchar(100)", maxLength: 100, nullable: false),
                    vaccinedate = table.Column<DateTime>(name: "vaccine-date", type: "datetime(6)", nullable: false),
                    vaccineimg = table.Column<string>(name: "vaccine-img", type: "varchar(300)", maxLength: 300, nullable: false),
                    bovineid = table.Column<int>(name: "bovine-id", type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_vaccines", x => x.id);
                    table.ForeignKey(
                        name: "f-k_vaccines_bovines_bovine-id",
                        column: x => x.bovineid,
                        principalTable: "bovines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "longtext", nullable: false),
                    details = table.Column<string>(type: "longtext", nullable: false),
                    campaignid = table.Column<int>(name: "campaign-id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_channels", x => x.id);
                    table.ForeignKey(
                        name: "f-k_channels_campaigns_campaign-id",
                        column: x => x.campaignid,
                        principalTable: "campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "goals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    metric = table.Column<string>(type: "longtext", nullable: false),
                    targetvalue = table.Column<int>(name: "target-value", type: "int", nullable: false),
                    currentvalue = table.Column<int>(name: "current-value", type: "int", nullable: false),
                    campaignid = table.Column<int>(name: "campaign-id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_goals", x => x.id);
                    table.ForeignKey(
                        name: "f-k_goals_campaigns_campaign-id",
                        column: x => x.campaignid,
                        principalTable: "campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    employee_status = table.Column<int>(type: "int", nullable: false),
                    campaignid = table.Column<int>(name: "campaign-id", type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p-k_staff", x => x.id);
                    table.ForeignKey(
                        name: "f-k_staff_campaigns_campaign-id",
                        column: x => x.campaignid,
                        principalTable: "campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i-x_bovines_stable-id",
                table: "bovines",
                column: "stable-id");

            migrationBuilder.CreateIndex(
                name: "i-x_campaigns_stable-id",
                table: "campaigns",
                column: "stable-id");

            migrationBuilder.CreateIndex(
                name: "i-x_channels_campaign-id",
                table: "channels",
                column: "campaign-id");

            migrationBuilder.CreateIndex(
                name: "i-x_goals_campaign-id",
                table: "goals",
                column: "campaign-id");

            migrationBuilder.CreateIndex(
                name: "i-x_staff_campaign-id",
                table: "staff",
                column: "campaign-id");

            migrationBuilder.CreateIndex(
                name: "i-x_vaccines_bovine-id",
                table: "vaccines",
                column: "bovine-id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "channels");

            migrationBuilder.DropTable(
                name: "credentials");

            migrationBuilder.DropTable(
                name: "goals");

            migrationBuilder.DropTable(
                name: "microsoft-credentials");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vaccines");

            migrationBuilder.DropTable(
                name: "campaigns");

            migrationBuilder.DropTable(
                name: "bovines");

            migrationBuilder.DropTable(
                name: "stables");
        }
    }
}
