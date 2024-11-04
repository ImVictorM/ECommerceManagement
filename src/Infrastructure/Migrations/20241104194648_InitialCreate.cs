using System;
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
            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => x.id);
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
                name: "shipment_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    id_product_category = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_product_categories_id_product_category",
                        column: x => x.id_product_category,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    total = table.Column<float>(type: "real", nullable: false),
                    id_user = table.Column<long>(type: "bigint", nullable: false),
                    postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    street = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    neighborhood = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    state = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    city = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    id_order_status = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_orders_users_id_user",
                        column: x => x.id_user,
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
                name: "product_discounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    percentage = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    starting_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ending_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_discounts_products_id_product",
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
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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
                name: "installments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quantity_payments = table.Column<int>(type: "integer", nullable: false),
                    amount_per_payment = table.Column<float>(type: "real", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installments", x => x.id);
                    table.ForeignKey(
                        name: "FK_installments_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_discounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    percentage = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    starting_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ending_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_discounts_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
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
                name: "orders_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price_on_order = table.Column<float>(type: "real", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    id_product = table.Column<long>(type: "bigint", nullable: false),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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
                name: "payments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    amount = table.Column<float>(type: "real", nullable: false),
                    id_installment = table.Column<long>(type: "bigint", nullable: true),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    id_payment_method = table.Column<long>(type: "bigint", nullable: false),
                    id_payment_status = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_installments_id_installment",
                        column: x => x.id_installment,
                        principalTable: "installments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_payments_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_payment_methods_id_payment_method",
                        column: x => x.id_payment_method,
                        principalTable: "payment_methods",
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

            migrationBuilder.CreateTable(
                name: "payment_status_histories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_payment_status = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    id_payment = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_status_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_status_histories_payment_statuses_id_payment_status",
                        column: x => x.id_payment_status,
                        principalTable: "payment_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payment_status_histories_payments_id_payment",
                        column: x => x.id_payment,
                        principalTable: "payments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "order_statuses",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 88, DateTimeKind.Unspecified).AddTicks(9982), new TimeSpan(0, 0, 0, 0, 0)), "pending", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(428), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(709), new TimeSpan(0, 0, 0, 0, 0)), "paid", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(710), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(720), new TimeSpan(0, 0, 0, 0, 0)), "shipped", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(720), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(733), new TimeSpan(0, 0, 0, 0, 0)), "delivered", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(733), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(785), new TimeSpan(0, 0, 0, 0, 0)), "canceled", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 89, DateTimeKind.Unspecified).AddTicks(785), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "payment_methods",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4189), new TimeSpan(0, 0, 0, 0, 0)), "credit_card", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4192), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4216), new TimeSpan(0, 0, 0, 0, 0)), "pix", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4217), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4235), new TimeSpan(0, 0, 0, 0, 0)), "bank_transfer", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4235), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4307), new TimeSpan(0, 0, 0, 0, 0)), "cash", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 104, DateTimeKind.Unspecified).AddTicks(4307), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "payment_statuses",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1520), new TimeSpan(0, 0, 0, 0, 0)), "pending", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1524), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1694), new TimeSpan(0, 0, 0, 0, 0)), "completed", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1695), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1705), new TimeSpan(0, 0, 0, 0, 0)), "failed", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1705), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1716), new TimeSpan(0, 0, 0, 0, 0)), "refunded", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 106, DateTimeKind.Unspecified).AddTicks(1716), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "product_categories",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5797), new TimeSpan(0, 0, 0, 0, 0)), "electronics", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5800), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5839), new TimeSpan(0, 0, 0, 0, 0)), "home_appliances", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5839), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5851), new TimeSpan(0, 0, 0, 0, 0)), "fashion", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5851), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5862), new TimeSpan(0, 0, 0, 0, 0)), "footwear", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5862), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5872), new TimeSpan(0, 0, 0, 0, 0)), "beauty", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5872), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 6L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5888), new TimeSpan(0, 0, 0, 0, 0)), "health_wellness", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5888), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 7L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5900), new TimeSpan(0, 0, 0, 0, 0)), "groceries", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5900), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 8L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5911), new TimeSpan(0, 0, 0, 0, 0)), "furniture", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5911), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 9L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5924), new TimeSpan(0, 0, 0, 0, 0)), "toys_games", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5924), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 10L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5940), new TimeSpan(0, 0, 0, 0, 0)), "books_stationery", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5941), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 11L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5956), new TimeSpan(0, 0, 0, 0, 0)), "sports_outdoor", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(5956), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 12L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6006), new TimeSpan(0, 0, 0, 0, 0)), "automotive", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6006), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 13L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6021), new TimeSpan(0, 0, 0, 0, 0)), "pet_supplies", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6021), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 14L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6038), new TimeSpan(0, 0, 0, 0, 0)), "jewelry_watches", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6038), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 15L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6055), new TimeSpan(0, 0, 0, 0, 0)), "office_supplies", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6055), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 16L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6071), new TimeSpan(0, 0, 0, 0, 0)), "home_improvement", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6072), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 17L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6087), new TimeSpan(0, 0, 0, 0, 0)), "baby_products", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6087), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 18L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6103), new TimeSpan(0, 0, 0, 0, 0)), "travel_luggage", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6103), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 19L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6122), new TimeSpan(0, 0, 0, 0, 0)), "music_instruments", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 109, DateTimeKind.Unspecified).AddTicks(6122), new TimeSpan(0, 0, 0, 0, 0)) }
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
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5528), new TimeSpan(0, 0, 0, 0, 0)), "pending", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5531), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5560), new TimeSpan(0, 0, 0, 0, 0)), "shipped", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5560), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5575), new TimeSpan(0, 0, 0, 0, 0)), "in_route", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5575), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5588), new TimeSpan(0, 0, 0, 0, 0)), "delivered", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5588), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5L, new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5600), new TimeSpan(0, 0, 0, 0, 0)), "canceled", new DateTimeOffset(new DateTime(2024, 11, 4, 19, 46, 48, 159, DateTimeKind.Unspecified).AddTicks(5600), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_installments_id_order",
                table: "installments",
                column: "id_order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventories_id_product",
                table: "inventories",
                column: "id_product",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_discounts_id_order",
                table: "order_discounts",
                column: "id_order");

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
                name: "IX_orders_id_user",
                table: "orders",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_orders_products_id_order",
                table: "orders_products",
                column: "id_order");

            migrationBuilder.CreateIndex(
                name: "IX_orders_products_id_product",
                table: "orders_products",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_name",
                table: "payment_methods",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_status_histories_id_payment",
                table: "payment_status_histories",
                column: "id_payment");

            migrationBuilder.CreateIndex(
                name: "IX_payment_status_histories_id_payment_status",
                table: "payment_status_histories",
                column: "id_payment_status");

            migrationBuilder.CreateIndex(
                name: "IX_payment_statuses_name",
                table: "payment_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_id_installment",
                table: "payments",
                column: "id_installment");

            migrationBuilder.CreateIndex(
                name: "IX_payments_id_order",
                table: "payments",
                column: "id_order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_id_payment_method",
                table: "payments",
                column: "id_payment_method");

            migrationBuilder.CreateIndex(
                name: "IX_payments_id_payment_status",
                table: "payments",
                column: "id_payment_status");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_name",
                table: "product_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_discounts_id_product",
                table: "product_discounts",
                column: "id_product");

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
                name: "IX_products_id_product_category",
                table: "products",
                column: "id_product_category");

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name",
                unique: true);

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
                name: "order_discounts");

            migrationBuilder.DropTable(
                name: "order_status_histories");

            migrationBuilder.DropTable(
                name: "orders_products");

            migrationBuilder.DropTable(
                name: "payment_status_histories");

            migrationBuilder.DropTable(
                name: "product_discounts");

            migrationBuilder.DropTable(
                name: "product_feedbacks");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "shipment_status_histories");

            migrationBuilder.DropTable(
                name: "user_addresses");

            migrationBuilder.DropTable(
                name: "users_roles");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "shipments");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "installments");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropTable(
                name: "payment_statuses");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "shipment_statuses");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
