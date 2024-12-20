using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.CouponRestrictionAggregate.DTOs;

/// <summary>
/// Represents a product input.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="CategoryIds">The product category ids.</param>
public record ContextProduct(ProductId ProductId, IEnumerable<CategoryId> CategoryIds);
