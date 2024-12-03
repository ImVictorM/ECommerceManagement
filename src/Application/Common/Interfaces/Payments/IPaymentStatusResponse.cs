namespace Application.Common.Interfaces.Payments;

/// <summary>
/// Represents a general payment response containing details about the payment.
/// </summary>
public interface IPaymentStatusResponse
{
    /// <summary>
    /// The current status of the payment.
    /// </summary>
    public string Status { get; }
    /// <summary>
    /// The details of the payment.
    /// </summary>
    public string Details { get; }
    /// <summary>
    /// A boolean value indicating if the payment was captured or not.
    /// </summary>
    public bool Captured { get; }
}
