namespace IntegrationTests.Common.Seeds.ProductReviews;

/// <summary>
/// Represents the types of product reviews available in the database seed.
/// </summary>
public enum ProductReviewSeedType
{
    /// <summary>
    /// Represents the first review entry for the
    /// <see cref="Products.ProductSeedType.COMPUTER_ON_SALE"/> product.
    /// </summary>
    COMPUTER_ON_SALE_REVIEW_1,

    /// <summary>
    /// Represents the first review entry for the.
    /// <see cref="Products.ProductSeedType.PENCIL"/> product.
    /// </summary>
    PENCIL_REVIEW_1,

    /// <summary>
    /// Represents the second review entry for the.
    /// <see cref="Products.ProductSeedType.PENCIL"/> product.
    /// </summary>
    PENCIL_REVIEW_2,

    /// <summary>
    /// Represents the first review entry for the.
    /// <see cref="Products.ProductSeedType.TSHIRT"/> product.
    /// </summary>
    TSHIRT_REVIEW_1,

    /// <summary>
    /// Represents the second and inactive review entry for the
    /// <see cref="Products.ProductSeedType.TSHIRT"/> product.
    /// </summary>
    TSHIRT_REVIEW_2_INACTIVE,

    /// <summary>
    /// Represents the first review entry for the.
    /// <see cref="Products.ProductSeedType.CHAIN_BRACELET"/> product.
    /// </summary>
    CHAIN_BRACELET_REVIEW_1,

    /// <summary>
    /// Represents the first and inactive review entry for the
    /// <see cref="Products.ProductSeedType.JACKET_INACTIVE"/> inactive
    /// product.
    /// </summary>
    JACKET_INACTIVE_REVIEW_1_INACTIVE
}
