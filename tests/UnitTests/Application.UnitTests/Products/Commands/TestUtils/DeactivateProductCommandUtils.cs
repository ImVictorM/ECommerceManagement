using Application.Products.Commands.DeactivateProduct;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Products.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeactivateProductCommand"/> command.
/// </summary>
public static class DeactivateProductCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeactivateProductCommand"/> class.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="DeactivateProductCommand"/> class.
    /// </returns>
    public static DeactivateProductCommand CreateCommand(
        string? id = null
    )
    {
        return new DeactivateProductCommand(
            id ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
