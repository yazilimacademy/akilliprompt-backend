using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AkilliPrompt.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "application_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "application_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    normalized_email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prompts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_by_user_id = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "text", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prompts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "application_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_role_claims_application_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "application_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_user_claims_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_application_user_logins_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_application_user_roles_application_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "application_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_application_user_roles_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(191)", maxLength: 191, nullable: false),
                    name = table.Column<string>(type: "character varying(191)", maxLength: 191, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_application_user_tokens_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_social_media_accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    social_media_type = table.Column<short>(type: "smallint", nullable: false),
                    url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_social_media_accounts", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_social_media_accounts_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "placeholders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    prompt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_placeholders", x => x.id);
                    table.ForeignKey(
                        name: "fk_placeholders_prompts_prompt_id",
                        column: x => x.prompt_id,
                        principalTable: "prompts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prompt_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    prompt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prompt_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_prompt_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prompt_categories_prompts_prompt_id",
                        column: x => x.prompt_id,
                        principalTable: "prompts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_favorite_prompts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prompt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_favorite_prompts", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_favorite_prompts_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_favorite_prompts_prompts_prompt_id",
                        column: x => x.prompt_id,
                        principalTable: "prompts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_like_prompts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prompt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_like_prompts", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_like_prompts_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_like_prompts_prompts_prompt_id",
                        column: x => x.prompt_id,
                        principalTable: "prompts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_prompt_comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    prompt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_comment_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_prompt_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_prompt_comments_application_users_user_id",
                        column: x => x.user_id,
                        principalTable: "application_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_prompt_comments_prompts_prompt_id",
                        column: x => x.prompt_id,
                        principalTable: "prompts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_prompt_comments_user_prompt_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "user_prompt_comments",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "application_roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("019358eb-f6cb-78c6-b59c-848777da66af"), "019358ec-42e0-70ba-8049-655ecc8e2d2e", "Admin", "ADMIN" },
                    { new Guid("019358ec-9d53-7785-a270-e22e10677a63"), "019358ec-aedc-742c-b677-a6b6bd8ef3bb", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "application_users",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "created_at", "created_by_user_id", "email", "email_confirmed", "lockout_enabled", "lockout_end", "modified_at", "modified_by_user_id", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[,]
                {
                    { new Guid("019358e7-cfd6-7ce0-a572-55f7859864b9"), 0, "b68dace3-2808-4be0-9a5d-637c5f2cfb09", new DateTimeOffset(new DateTime(2024, 11, 23, 15, 11, 4, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "019358e7-cfd6-7ce0-a572-55f7859864b9", "alper.tunga@yazilim.academy", true, false, null, null, null, "ALPER.TUNGA@YAZILIM.ACADEMY", "ALPERTUNGA", null, null, false, "0c74dcdd-892a-41a3-995d-92c8529529dc", false, "alpertunga" },
                    { new Guid("019358ef-0146-7bf9-994b-880e1002a653"), 0, "019358ef-3691-7726-844a-c0d9979417f4", new DateTimeOffset(new DateTime(2024, 11, 23, 15, 11, 5, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "019358ef-0146-7bf9-994b-880e1002a653", "merveeksii61@gmail.com", true, false, null, null, null, "MERVEEKSII61@GMAIL.COM", "MERVEEKSI", null, null, false, "019358ef-213d-7ada-b02d-4e60dd64b9f9", false, "merveeksi" }
                });

            migrationBuilder.InsertData(
                table: "application_user_roles",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { new Guid("019358eb-f6cb-78c6-b59c-848777da66af"), new Guid("019358e7-cfd6-7ce0-a572-55f7859864b9") },
                    { new Guid("019358ec-9d53-7785-a270-e22e10677a63"), new Guid("019358ef-0146-7bf9-994b-880e1002a653") }
                });

            migrationBuilder.CreateIndex(
                name: "ix_application_role_claims_role_id",
                table: "application_role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "role_name_index",
                table: "application_roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_application_user_claims_user_id",
                table: "application_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_user_logins_user_id",
                table: "application_user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_user_roles_role_id",
                table: "application_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "email_index",
                table: "application_users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_application_users_email",
                table: "application_users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_name_index",
                table: "application_users",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_placeholders_prompt_id",
                table: "placeholders",
                column: "prompt_id");

            migrationBuilder.CreateIndex(
                name: "ix_prompt_categories_category_id",
                table: "prompt_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_prompt_categories_prompt_id_category_id",
                table: "prompt_categories",
                columns: new[] { "prompt_id", "category_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_favorite_prompts_prompt_id",
                table: "user_favorite_prompts",
                column: "prompt_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_favorite_prompts_user_id_prompt_id",
                table: "user_favorite_prompts",
                columns: new[] { "user_id", "prompt_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_like_prompts_prompt_id",
                table: "user_like_prompts",
                column: "prompt_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_like_prompts_user_id_prompt_id",
                table: "user_like_prompts",
                columns: new[] { "user_id", "prompt_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_prompt_comments_parent_comment_id",
                table: "user_prompt_comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_prompt_comments_prompt_id",
                table: "user_prompt_comments",
                column: "prompt_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_prompt_comments_user_id",
                table: "user_prompt_comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_social_media_accounts_user_id",
                table: "user_social_media_accounts",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_role_claims");

            migrationBuilder.DropTable(
                name: "application_user_claims");

            migrationBuilder.DropTable(
                name: "application_user_logins");

            migrationBuilder.DropTable(
                name: "application_user_roles");

            migrationBuilder.DropTable(
                name: "application_user_tokens");

            migrationBuilder.DropTable(
                name: "placeholders");

            migrationBuilder.DropTable(
                name: "prompt_categories");

            migrationBuilder.DropTable(
                name: "user_favorite_prompts");

            migrationBuilder.DropTable(
                name: "user_like_prompts");

            migrationBuilder.DropTable(
                name: "user_prompt_comments");

            migrationBuilder.DropTable(
                name: "user_social_media_accounts");

            migrationBuilder.DropTable(
                name: "application_roles");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "prompts");

            migrationBuilder.DropTable(
                name: "application_users");
        }
    }
}
