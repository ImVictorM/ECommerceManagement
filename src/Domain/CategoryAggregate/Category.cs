using Domain.CategoryAggregate.ValueObjects;
using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.CategoryAggregate;

/// <summary>
/// Represents a category.
/// </summary>
public class Category : AggregateRoot<CategoryId>
{
    /// <summary>
    /// Gets the category name.
    /// </summary>
    public string Name { get; private set; } = null!;

    private Category() { }

    private Category(string name)
    {
        Name = name.ToLowerSnakeCase();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>A new instance of the <see cref="Category"/> class.</returns>
    public static Category Create(string name)
    {
        return new Category(name);
    }

    /// <summary>
    /// Updates the current category.
    /// </summary>
    /// <param name="name">The new category name.</param>
    public void Update(string name)
    {
        Name = name.ToLowerSnakeCase();
    }
}
