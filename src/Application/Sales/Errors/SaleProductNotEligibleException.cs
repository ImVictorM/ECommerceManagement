using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Errors;

namespace Application.Sales.Errors;

/// <summary>
/// Exception thrown when a product does not meet the eligibility criteria
/// for a sale.
/// </summary>
public class SaleProductNotEligibleException : BaseException
{
    private const string DefaultTitle = "Sale Eligibility Violation";
    private const string DefaultMessage =
        "Some of the sale products is not eligible for the applied sale";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    internal SaleProductNotEligibleException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal SaleProductNotEligibleException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal SaleProductNotEligibleException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }

    internal SaleProductNotEligibleException(ProductId productId)
        : base(
            $"The product with id '{productId}' does not meet the eligibility " +
            $"requirements for the sale.",
            DefaultTitle,
            _defaultErrorCode
        )
    {
    }
}
