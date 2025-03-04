using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;

namespace IntegrationTests.Common.Seeds.ProductFeedback;

/// <summary>
/// Provides seed data for product feedback in the database.
/// </summary>
public sealed class ProductFeedbackSeed :
    DataSeed<ProductFeedbackSeedType, DomainProductFeedback, ProductFeedbackId>,
    IProductFeedbackSeed
{
    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductFeedbackSeed"/> class.
    /// </summary>
    /// <param name="productSeed">The product seed.</param>
    /// <param name="userSeed">The user seed.</param>
    public ProductFeedbackSeed(IProductSeed productSeed, IUserSeed userSeed)
        : base(CreateSeedData(productSeed, userSeed))
    {
        Order = productSeed.Order + userSeed.Order + 20;
    }

    private static Dictionary<
        ProductFeedbackSeedType,
        DomainProductFeedback
    > CreateSeedData(IProductSeed productSeed, IUserSeed userSeed)
    {
        return new()
        {
            [ProductFeedbackSeedType.COMPUTER_ON_SALE_FEEDBACK_1] = ProductFeedbackUtils
                .CreateProductFeedback(
                    id: ProductFeedbackId.Create(-1),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.COMPUTER_ON_SALE),
                    title: "Great value but has some flaws",
                    content: "The computer is fast and works well for the price," +
                    " Although the fan can get quite loud under heavy use.",
                    starRating: StarRating.Create(4)
                ),

            [ProductFeedbackSeedType.PENCIL_FEEDBACK_1] = ProductFeedbackUtils
                .CreateProductFeedback(
                    id: ProductFeedbackId.Create(-2),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER_WITH_ADDRESS),
                    productId: productSeed.GetEntityId(ProductSeedType.PENCIL),
                    title: "Decent pencils, but not the best",
                    content: "These pencils are okay for basic writing, but the lead" +
                    " tends to break easily when sharpening. " +
                    "They're affordable, but I've had better.",
                    starRating: StarRating.Create(3)
                ),

            [ProductFeedbackSeedType.PENCIL_FEEDBACK_2] = ProductFeedbackUtils
                .CreateProductFeedback(
                    id: ProductFeedbackId.Create(-3),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.PENCIL),
                    title: "Perfect for everyday use",
                    content: "I use these pencils daily, and they work great! " +
                    "Smooth writing, easy to sharpen, and they last a long time. " +
                    "Highly recommend!",
                    starRating: StarRating.Create(5)
                ),

            [ProductFeedbackSeedType.TSHIRT_FEEDBACK_1] = ProductFeedbackUtils
                .CreateProductFeedback(
                    id: ProductFeedbackId.Create(-4),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER_WITH_ADDRESS),
                    productId: productSeed.GetEntityId(ProductSeedType.TSHIRT),
                    title: "Comfortable but shrinks after washing",
                    content: "The t-shirt is soft and fits well initially, but it " +
                    "shrunk significantly after the first wash. " +
                    "Disappointed with the durability.",
                    starRating: StarRating.Create(2)
                ),

            [ProductFeedbackSeedType.TSHIRT_FEEDBACK_2_INACTIVE] = ProductFeedbackUtils
                .CreateProductFeedback(
                    id: ProductFeedbackId.Create(-5),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.TSHIRT),
                    title: "AAAAAAAAA",
                    content: "I do not know why I am leaving this review.",
                    starRating: StarRating.Create(1)
                ),

            [ProductFeedbackSeedType.CHAIN_BRACELET_FEEDBACK_1] = ProductFeedbackUtils
                .CreateProductFeedback(
                    id: ProductFeedbackId.Create(-6),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.CHAIN_BRACELET),
                    title: "Beautiful but fragile",
                    content: "The bracelet looks stunning, but the chain broke after just" +
                    " a week of wear. " +
                    "It's not durable enough for daily use.",
                    starRating: StarRating.Create(2)
                ),
            [ProductFeedbackSeedType.JACKET_INACTIVE_FEEDBACK_1_INACTIVE] = ProductFeedbackUtils
                .CreateProductFeedback(
                    id: ProductFeedbackId.Create(-7),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.CHAIN_BRACELET),
                    title: "Came in the wrong color",
                    content: "The jacket is good, but I did no order an orange one.",
                    starRating: StarRating.Create(3)
                )
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.ProductFeedback.AddRangeAsync(ListAll());
    }
}
