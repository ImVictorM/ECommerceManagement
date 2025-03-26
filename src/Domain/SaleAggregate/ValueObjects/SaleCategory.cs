using Domain.CategoryAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a category that is associated with a sale.
/// </summary>
public class SaleCategory : ValueObject
{
    /// <summary>
    /// Gets the category identifier.
    /// </summary>
    public CategoryId CategoryId { get; } = null!;

    private SaleCategory() { }

    private SaleCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleCategory"/> class.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="SaleCategory"/> class.
    /// </returns>
    public static SaleCategory Create(CategoryId categoryId)
    {
        return new SaleCategory(categoryId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoryId;
    }
}
