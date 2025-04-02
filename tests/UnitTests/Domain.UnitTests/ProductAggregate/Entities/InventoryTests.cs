using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.Errors;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ProductAggregate.Entities;

/// <summary>
/// Unit tests for the <see cref="Inventory"/> entity.
/// </summary>
public class InventoryTests
{
    /// <summary>
    /// Verifies that is possible to increment the product's inventory quantity
    /// available.
    /// </summary>
    [Theory]
    [InlineData(10, 55, 65)]
    [InlineData(20, 5, 25)]
    [InlineData(12, 1, 13)]
    public void AddStock_WithPositiveValue_IncrementsQuantityAvailable(
        int initialQuantity,
        int quantityToAdd,
        int expectedQuantityAvailable
    )
    {
        var inventory = Inventory.Create(initialQuantity);

        inventory.AddStock(quantityToAdd);

        inventory.QuantityAvailable.Should().Be(expectedQuantityAvailable);
    }

    /// <summary>
    /// Verifies it is possible to remove items from stock when quantity available
    /// is sufficient.
    /// </summary>
    [Fact]
    public void RemoveStock_WithSufficientStock_DecrementsQuantityAvailable()
    {
        var product = ProductUtils.CreateProduct(initialQuantityInInventory: 100);

        product.Inventory.RemoveStock(20);

        product.Inventory.QuantityAvailable.Should().Be(80);
    }

    /// <summary>
    /// Verifies removing items from stock throws an error when the quantity
    /// available is not sufficient.
    /// </summary>
    [Fact]
    public void RemoveStock_WithInsufficientStock_ThrowsError()
    {
        var product = ProductUtils.CreateProduct(initialQuantityInInventory: 10);

        FluentActions
            .Invoking(() => product.Inventory.RemoveStock(20))
            .Should()
            .Throw<InventoryInsufficientException>();
    }

    /// <summary>
    /// Verifies clearing the inventory stock sets the quantity available to zero.
    /// </summary>
    [Fact]
    public void ClearStock_WithQuantityAvailable_SetsQuantityAvailableToZero()
    {
        var product = ProductUtils.CreateProduct(initialQuantityInInventory: 200);

        product.Inventory.ClearStock();

        product.Inventory.QuantityAvailable.Should().Be(0);
    }
}
