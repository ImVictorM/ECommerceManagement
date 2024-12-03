namespace Application.Common.Interfaces.Payments;

/// <summary>
/// Represents a response for authorize payment requests.
/// </summary>
public interface IAuthorizePaymentResponse : IPaymentStatusResponse
{
    /// <summary>
    /// The payment identifier.
    /// </summary>
    public string PaymentId { get; }
}
