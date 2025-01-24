using Domain.CategoryAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a category in sale.
/// </summary>
public class CategoryReference : ValueObject
{
    /// <summary>
    /// Gets the category id.
    /// </summary>
    public CategoryId CategoryId { get; } = null!;

    private CategoryReference() { }

    private CategoryReference(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CategoryReference"/> class.
    /// </summary>
    /// <param name="categoryId">The category id.</param>
    /// <returns>A new instance of the <see cref="CategoryReference"/> class.</returns>
    public static CategoryReference Create(CategoryId categoryId)
    {
        return new CategoryReference(categoryId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoryId;
    }
}
