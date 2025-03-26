namespace Domain.SaleAggregate.Services;

/// <summary>
/// Defines services for validating whether a sale is eligible based on its
/// discount and affected products.
/// </summary>
public interface ISaleEligibilityService
{
    /// <summary>
    /// Ensures that all products included in a sale meet the eligibility criteria.
    /// Throws an exception if any product violates discount restrictions.
    /// </summary>
    /// <param name="sale">The sale to be validated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task EnsureSaleProductsEligibilityAsync(
        Sale sale,
        CancellationToken cancellationToken
    );
}
