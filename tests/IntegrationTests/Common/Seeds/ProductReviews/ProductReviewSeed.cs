using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductReviewAggregate;
using Domain.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;

namespace IntegrationTests.Common.Seeds.ProductReviews;

/// <summary>
/// Provides seed data for product reviews in the database.
/// </summary>
public sealed class ProductReviewSeed :
    DataSeed<ProductReviewSeedType, ProductReview, ProductReviewId>,
    IProductReviewSeed
{
    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductReviewSeed"/> class.
    /// </summary>
    /// <param name="productSeed">The product seed.</param>
    /// <param name="userSeed">The user seed.</param>
    public ProductReviewSeed(IProductSeed productSeed, IUserSeed userSeed)
        : base(CreateSeedData(productSeed, userSeed))
    {
        Order = productSeed.Order + userSeed.Order + 20;
    }

    private static Dictionary<
        ProductReviewSeedType,
        ProductReview
    > CreateSeedData(IProductSeed productSeed, IUserSeed userSeed)
    {
        return new()
        {
            [ProductReviewSeedType.COMPUTER_ON_SALE_REVIEW_1] = ProductReviewUtils
                .CreateProductReview(
                    id: ProductReviewId.Create(-1),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed
                        .GetEntityId(ProductSeedType.COMPUTER_ON_SALE),
                    title: "Great value but has some flaws",
                    content: "The computer is fast and works well for the price," +
                    " Although the fan can get quite loud under heavy use.",
                    starRating: StarRating.Create(4)
                ),

            [ProductReviewSeedType.PENCIL_REVIEW_1] = ProductReviewUtils
                .CreateProductReview(
                    id: ProductReviewId.Create(-2),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER_WITH_ADDRESS),
                    productId: productSeed.GetEntityId(ProductSeedType.PENCIL),
                    title: "Decent pencils, but not the best",
                    content: "These pencils are okay for basic writing, but the lead" +
                    " tends to break easily when sharpening. " +
                    "They're affordable, but I've had better.",
                    starRating: StarRating.Create(3)
                ),

            [ProductReviewSeedType.PENCIL_REVIEW_2] = ProductReviewUtils
                .CreateProductReview(
                    id: ProductReviewId.Create(-3),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.PENCIL),
                    title: "Perfect for everyday use",
                    content: "I use these pencils daily, and they work great! " +
                    "Smooth writing, easy to sharpen, and they last a long time. " +
                    "Highly recommend!",
                    starRating: StarRating.Create(5)
                ),

            [ProductReviewSeedType.TSHIRT_REVIEW_1] = ProductReviewUtils
                .CreateProductReview(
                    id: ProductReviewId.Create(-4),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER_WITH_ADDRESS),
                    productId: productSeed.GetEntityId(ProductSeedType.TSHIRT),
                    title: "Comfortable but shrinks after washing",
                    content: "The t-shirt is soft and fits well initially, but it " +
                    "shrunk significantly after the first wash. " +
                    "Disappointed with the durability.",
                    starRating: StarRating.Create(2)
                ),

            [ProductReviewSeedType.TSHIRT_REVIEW_2_INACTIVE] = ProductReviewUtils
                .CreateProductReview(
                    id: ProductReviewId.Create(-5),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.TSHIRT),
                    title: "AAAAAAAAA",
                    content: "I do not know why I am leaving this review.",
                    starRating: StarRating.Create(1)
                ),

            [ProductReviewSeedType.CHAIN_BRACELET_REVIEW_1] = ProductReviewUtils
                .CreateProductReview(
                    id: ProductReviewId.Create(-6),
                    userId: userSeed.GetEntityId(UserSeedType.CUSTOMER),
                    productId: productSeed.GetEntityId(ProductSeedType.CHAIN_BRACELET),
                    title: "Beautiful but fragile",
                    content: "The bracelet looks stunning, but the chain broke " +
                    "after just a week of wear. " +
                    "It's not durable enough for daily use.",
                    starRating: StarRating.Create(2)
                ),
            [ProductReviewSeedType.JACKET_INACTIVE_REVIEW_1_INACTIVE] = ProductReviewUtils
                .CreateProductReview(
                    id: ProductReviewId.Create(-7),
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
        await context.ProductReviews.AddRangeAsync(ListAll());
    }
}
