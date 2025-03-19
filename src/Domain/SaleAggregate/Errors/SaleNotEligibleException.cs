using SharedKernel.Errors;

namespace Domain.SaleAggregate.Errors;

/// <summary>
/// Exception thrown when a product does not meet the eligibility criteria
/// for a sale.
/// </summary>
public class SaleNotEligibleException : BaseException
{
    private const string DefaultTitle = "Sale Eligibility Violation";
    private const string DefaultMessage =
        "Some of the sale products is not eligible for the applied sale";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    internal SaleNotEligibleException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal SaleNotEligibleException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal SaleNotEligibleException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
