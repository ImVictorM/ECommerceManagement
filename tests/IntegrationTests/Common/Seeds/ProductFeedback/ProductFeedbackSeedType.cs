namespace IntegrationTests.Common.Seeds.ProductFeedback;

/// <summary>
/// Represents the types of product feedback available in the database seed.
/// </summary>
public enum ProductFeedbackSeedType
{
    /// <summary>
    /// Represents the first feedback entry for the
    /// <see cref="Products.ProductSeedType.COMPUTER_ON_SALE"/> product.
    /// </summary>
    COMPUTER_ON_SALE_FEEDBACK_1,

    /// <summary>
    /// Represents the first feedback entry for the.
    /// <see cref="Products.ProductSeedType.PENCIL"/> product.
    /// </summary>
    PENCIL_FEEDBACK_1,

    /// <summary>
    /// Represents the second feedback entry for the.
    /// <see cref="Products.ProductSeedType.PENCIL"/> product.
    /// </summary>
    PENCIL_FEEDBACK_2,

    /// <summary>
    /// Represents the first feedback entry for the.
    /// <see cref="Products.ProductSeedType.TSHIRT"/> product.
    /// </summary>
    TSHIRT_FEEDBACK_1,

    /// <summary>
    /// Represents the second and inactive feedback entry for the
    /// <see cref="Products.ProductSeedType.TSHIRT"/> product.
    /// </summary>
    TSHIRT_FEEDBACK_2_INACTIVE,

    /// <summary>
    /// Represents the first feedback entry for the.
    /// <see cref="Products.ProductSeedType.CHAIN_BRACELET"/> product.
    /// </summary>
    CHAIN_BRACELET_FEEDBACK_1,

    /// <summary>
    /// Represents the first and inactive feedback entry for the
    /// <see cref="Products.ProductSeedType.JACKET_INACTIVE"/> inactive
    /// product.
    /// </summary>
    JACKET_INACTIVE_FEEDBACK_1_INACTIVE
}
