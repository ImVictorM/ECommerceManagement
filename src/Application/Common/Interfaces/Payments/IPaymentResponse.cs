namespace Application.Common.Interfaces.Payments;

/// <summary>
/// Represents a response for authorize payment requests.
/// </summary>
public interface IPaymentResponse : IPaymentStatusResponse
{
    /// <summary>
    /// Gets the payment identifier.
    /// </summary>
    public string PaymentId { get; }
    /// <summary>
    /// Gets the payment method.
    /// </summary>
    public string PaymentMethod { get; }
    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    public decimal Amount { get; }
    /// <summary>
    /// Gets the payment installments.
    /// </summary>
    public int Installments { get; }
}
