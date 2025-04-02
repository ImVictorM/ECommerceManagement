using Application.Orders.DTOs.Results;
using Application.Orders.Queries.Projections;

using Domain.OrderAggregate.ValueObjects;

using FluentAssertions;

namespace Application.UnitTests.Orders.TestUtils.Extensions;

/// <summary>
/// Utilities for the <see cref="OrderResult"/> class.
/// </summary>
public static class OrderResultUtils
{
    /// <summary>
    /// Ensures an <see cref="OrderResult"/> matches the results
    /// from an <see cref="OrderProjection"/> projection.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="projection">The query projection.</param>
    public static void EnsureCorrespondsTo(
        this OrderResult result,
        OrderProjection projection
    )
    {
        result.Id.Should().Be(projection.Id.ToString());
        result.OwnerId.Should().Be(projection.OwnerId.ToString());
        result.Description.Should().Be(projection.Description);
        result.Status.Should().Be(projection.OrderStatus.Name);
        result.Total.Should().Be(projection.Total);
    }

    /// <summary>
    /// Ensures a collection of <see cref="OrderResult"/> matches the results
    /// from a collection of <see cref="OrderProjection"/>.
    /// </summary>
    /// <param name="results">The current results.</param>
    /// <param name="projections">The query projections.</param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<OrderResult> results,
        IEnumerable<OrderProjection> projections
    )
    {
        foreach (var result in results)
        {
            var resultProjection = projections
                .FirstOrDefault(p => p.Id == OrderId.Create(result.Id));

            resultProjection.Should().NotBeNull();
            result.EnsureCorrespondsTo(resultProjection!);
        }
    }
}
