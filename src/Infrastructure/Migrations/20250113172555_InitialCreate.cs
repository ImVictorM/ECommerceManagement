using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "CouponRestrictionSequence");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coupons",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    percentage = table.Column<int>(type: "integer", nullable: false),
                    starting_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ending_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    code = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    usage_limit = table.Column<int>(type: "integer", nullable: false),
                    auto_apply = table.Column<bool>(type: "boolean", nullable: false),
                    min_price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    base_price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    percentage = table.Column<int>(type: "integer", nullable: false),
                    starting_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ending_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shipment_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    phone = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    password_hash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "restriction_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"CouponRestrictionSequence\"')"),
                    id_coupon = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restriction_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_restriction_categories_coupons_id_coupon",
                        column: x => x.id_coupon,
                        principalTable: "coupons",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "restriction_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"CouponRestrictionSequence\"')"),
                    id_coupon = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restriction_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_restriction_products_coupons_id_coupon",
                        column: x => x.id_coupon,
                        principalTable: "coupons",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "inventories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quantity_available = table.Column<int>(type: "integer", nullable: false),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventories", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventories_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url = table.Column<string>(type: "text", nullable: false),
                    id_product = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_images_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_category = table.Column<long>(type: "bigint", nullable: false),
                    id_product = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_categories_categories_id_category",
                        column: x => x.id_category,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_categories_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sale_categories_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_category = table.Column<long>(type: "bigint", nullable: false),
                    id_sale = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_categories_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_sale_categories_categories_categories_id_category",
                        column: x => x.id_category,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sale_categories_categories_sales_id_sale",
                        column: x => x.id_sale,
                        principalTable: "sales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sale_excluded_products_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    id_sale = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_excluded_products_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_sale_excluded_products_products_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sale_excluded_products_products_sales_id_sale",
                        column: x => x.id_sale,
                        principalTable: "sales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sale_products_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    id_sale = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_products_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_sale_products_products_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sale_products_products_sales_id_sale",
                        column: x => x.id_sale,
                        principalTable: "sales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    id_owner = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    id_order_status = table.Column<long>(type: "bigint", nullable: false),
                    postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    street = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    neighborhood = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    state = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    city = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_order_statuses_id_order_status",
                        column: x => x.id_order_status,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_users_id_owner",
                        column: x => x.id_owner,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_addresses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    street = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    neighborhood = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    state = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    city = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    id_user = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_addresses_users_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_role = table.Column<long>(type: "bigint", nullable: false),
                    id_user = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_roles_id_role",
                        column: x => x.id_role,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_roles_users_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "restriction_categories_allowed_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_category = table.Column<long>(type: "bigint", nullable: false),
                    id_restriction_category = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restriction_categories_allowed_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_restriction_categories_allowed_categories_categories_id_cat~",
                        column: x => x.id_category,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_restriction_categories_allowed_categories_restriction_categ~",
                        column: x => x.id_restriction_category,
                        principalTable: "restriction_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "restriction_products_not_allowed_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    id_restriction_category = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restriction_products_not_allowed_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_restriction_products_not_allowed_products_products_id_produ~",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_restriction_products_not_allowed_products_restriction_categ~",
                        column: x => x.id_restriction_category,
                        principalTable: "restriction_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "restriction_products_allowed_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    id_restriction_product = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restriction_products_allowed_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_restriction_products_allowed_products_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_restriction_products_allowed_products_restriction_products_~",
                        column: x => x.id_restriction_product,
                        principalTable: "restriction_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_status_histories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_order_status = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_status_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_status_histories_order_statuses_id_order_status",
                        column: x => x.id_order_status,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_status_histories_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders_coupons",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_coupon = table.Column<long>(type: "bigint", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders_coupons", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_coupons_coupons_id_coupon",
                        column: x => x.id_coupon,
                        principalTable: "coupons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_coupons_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    base_price = table.Column<decimal>(type: "numeric", nullable: false),
                    purchased_price = table.Column<decimal>(type: "numeric", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_products_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_products_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    id_payment_status = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_payment_statuses_id_payment_status",
                        column: x => x.id_payment_status,
                        principalTable: "payment_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_feedbacks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    star_rating = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    id_user = table.Column<long>(type: "bigint", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_feedbacks_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_feedbacks_products_id_product",
                        column: x => x.id_product,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_feedbacks_users_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shipments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accountable = table.Column<string>(type: "text", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    id_shipment_status = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipments", x => x.id);
                    table.ForeignKey(
                        name: "FK_shipments_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shipments_shipment_statuses_id_shipment_status",
                        column: x => x.id_shipment_status,
                        principalTable: "shipment_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_product_category_ids",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<long>(type: "bigint", nullable: false),
                    id_order_product = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_product_category_ids", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_product_category_ids_orders_products_id_order_product",
                        column: x => x.id_order_product,
                        principalTable: "orders_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shipment_status_histories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_shipment_status = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    id_shipment = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_status_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_shipment_status_histories_shipment_statuses_id_shipment_sta~",
                        column: x => x.id_shipment_status,
                        principalTable: "shipment_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shipment_status_histories_shipments_id_shipment",
                        column: x => x.id_shipment,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "order_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1L, "pending" },
                    { 2L, "paid" },
                    { 3L, "shipped" },
                    { 4L, "delivered" },
                    { 5L, "canceled" }
                });

            migrationBuilder.InsertData(
                table: "payment_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1L, "pending" },
                    { 2L, "in_progress" },
                    { 3L, "authorized" },
                    { 4L, "approved" },
                    { 5L, "rejected" },
                    { 6L, "canceled" },
                    { 7L, "refunded" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1L, "admin" },
                    { 2L, "customer" }
                });

            migrationBuilder.InsertData(
                table: "shipment_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1L, "pending" },
                    { 2L, "shipped" },
                    { 3L, "in_route" },
                    { 4L, "delivered" },
                    { 5L, "canceled" }
                });

            // password: admin123
            migrationBuilder.InsertData(
                table: "users",
                columns: ["id", "name", "email", "password_hash", "is_active", "created_at", "updated_at"],
                values: [
                    1,
                    "admin",
                    "admin@email.com",
                    "6333824CC074E187E261A0CBBD91F9741B4D38A26E1519A93B4244BEAFC933B9-4FDE231393F2C8AECC2B26F356E3D89E",
                    true,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                ]
            );

            migrationBuilder.InsertData(
                table: "users_roles",
                columns: ["id_role", "id_user"],
                values: [
                    1,
                    1,
                ]
            );

            migrationBuilder.CreateIndex(
                name: "IX_categories_name",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coupons_code",
                table: "coupons",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventories_id_product",
                table: "inventories",
                column: "id_product",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_product_category_ids_id_order_product",
                table: "order_product_category_ids",
                column: "id_order_product");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_histories_id_order",
                table: "order_status_histories",
                column: "id_order");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_histories_id_order_status",
                table: "order_status_histories",
                column: "id_order_status");

            migrationBuilder.CreateIndex(
                name: "IX_order_statuses_name",
                table: "order_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_order_status",
                table: "orders",
                column: "id_order_status");

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_owner",
                table: "orders",
                column: "id_owner");

            migrationBuilder.CreateIndex(
                name: "IX_orders_coupons_id_coupon",
                table: "orders_coupons",
                column: "id_coupon");

            migrationBuilder.CreateIndex(
                name: "IX_orders_coupons_id_order",
                table: "orders_coupons",
                column: "id_order");

            migrationBuilder.CreateIndex(
                name: "IX_orders_products_id_order",
                table: "orders_products",
                column: "id_order");

            migrationBuilder.CreateIndex(
                name: "IX_orders_products_id_product",
                table: "orders_products",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_payment_statuses_name",
                table: "payment_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_id_order",
                table: "payments",
                column: "id_order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_id_payment_status",
                table: "payments",
                column: "id_payment_status");

            migrationBuilder.CreateIndex(
                name: "IX_product_feedbacks_id_order",
                table: "product_feedbacks",
                column: "id_order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_feedbacks_id_product",
                table: "product_feedbacks",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_product_feedbacks_id_user",
                table: "product_feedbacks",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_product_images_id_product",
                table: "product_images",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_products_categories_id_category",
                table: "products_categories",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_products_categories_id_product",
                table: "products_categories",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_categories_id_coupon",
                table: "restriction_categories",
                column: "id_coupon");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_categories_allowed_categories_id_category",
                table: "restriction_categories_allowed_categories",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_categories_allowed_categories_id_restriction_ca~",
                table: "restriction_categories_allowed_categories",
                column: "id_restriction_category");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_products_id_coupon",
                table: "restriction_products",
                column: "id_coupon");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_products_allowed_products_id_product",
                table: "restriction_products_allowed_products",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_products_allowed_products_id_restriction_product",
                table: "restriction_products_allowed_products",
                column: "id_restriction_product");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_products_not_allowed_products_id_product",
                table: "restriction_products_not_allowed_products",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_restriction_products_not_allowed_products_id_restriction_ca~",
                table: "restriction_products_not_allowed_products",
                column: "id_restriction_category");

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sale_categories_categories_id_category",
                table: "sale_categories_categories",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_sale_categories_categories_id_sale",
                table: "sale_categories_categories",
                column: "id_sale");

            migrationBuilder.CreateIndex(
                name: "IX_sale_excluded_products_products_id_product",
                table: "sale_excluded_products_products",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_sale_excluded_products_products_id_sale",
                table: "sale_excluded_products_products",
                column: "id_sale");

            migrationBuilder.CreateIndex(
                name: "IX_sale_products_products_id_product",
                table: "sale_products_products",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_sale_products_products_id_sale",
                table: "sale_products_products",
                column: "id_sale");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_status_histories_id_shipment",
                table: "shipment_status_histories",
                column: "id_shipment");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_status_histories_id_shipment_status",
                table: "shipment_status_histories",
                column: "id_shipment_status");

            migrationBuilder.CreateIndex(
                name: "IX_shipment_statuses_name",
                table: "shipment_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shipments_id_order",
                table: "shipments",
                column: "id_order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shipments_id_shipment_status",
                table: "shipments",
                column: "id_shipment_status");

            migrationBuilder.CreateIndex(
                name: "IX_user_addresses_id_user",
                table: "user_addresses",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_roles_id_role",
                table: "users_roles",
                column: "id_role");

            migrationBuilder.CreateIndex(
                name: "IX_users_roles_id_user",
                table: "users_roles",
                column: "id_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventories");

            migrationBuilder.DropTable(
                name: "order_product_category_ids");

            migrationBuilder.DropTable(
                name: "order_status_histories");

            migrationBuilder.DropTable(
                name: "orders_coupons");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "product_feedbacks");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "products_categories");

            migrationBuilder.DropTable(
                name: "restriction_categories_allowed_categories");

            migrationBuilder.DropTable(
                name: "restriction_products_allowed_products");

            migrationBuilder.DropTable(
                name: "restriction_products_not_allowed_products");

            migrationBuilder.DropTable(
                name: "sale_categories_categories");

            migrationBuilder.DropTable(
                name: "sale_excluded_products_products");

            migrationBuilder.DropTable(
                name: "sale_products_products");

            migrationBuilder.DropTable(
                name: "shipment_status_histories");

            migrationBuilder.DropTable(
                name: "user_addresses");

            migrationBuilder.DropTable(
                name: "users_roles");

            migrationBuilder.DropTable(
                name: "orders_products");

            migrationBuilder.DropTable(
                name: "payment_statuses");

            migrationBuilder.DropTable(
                name: "restriction_products");

            migrationBuilder.DropTable(
                name: "restriction_categories");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "sales");

            migrationBuilder.DropTable(
                name: "shipments");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "coupons");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "shipment_statuses");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropSequence(
                name: "CouponRestrictionSequence");
        }
    }
}
