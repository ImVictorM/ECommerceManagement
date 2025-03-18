using Domain.CategoryAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a category that is associated with a sale.
/// </summary>
public class SaleCategory : ValueObject
{
    /// <summary>
    /// Gets the category id.
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
    /// <param name="categoryId">The category id.</param>
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
