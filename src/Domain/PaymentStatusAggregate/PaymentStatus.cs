using Domain.Common.Models;
using Domain.PaymentStatusAggregate.ValueObjects;

namespace Domain.PaymentStatusAggregate;

/// <summary>
/// Represents the payment.
/// </summary>
public sealed class PaymentStatus : AggregateRoot<PaymentStatusId>
{
    /// <summary>
    /// Gets the status of the payment.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    private PaymentStatus() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    /// <param name="status">The payment status.</param>
    private PaymentStatus(string status)
    {
        Status = status;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    /// <param name="status">The payment status.</param>
    /// <returns>A new instance of the <see cref="PaymentStatus"/> class.</returns>
    public static PaymentStatus Create(string status)
    {
        return new PaymentStatus(status);
    }
}
