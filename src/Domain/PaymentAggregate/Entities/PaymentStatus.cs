using Domain.Common.Models;
using Domain.PaymentAggregate.ValueObjects;

namespace Domain.PaymentAggregate.Entities;

/// <summary>
/// Represents the payment status of a <see cref="Payment"/>.
/// </summary>
public sealed class PaymentStatus : Entity<PaymentStatusId>
{
    /// <summary>
    /// Gets the status of the payment.
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    /// <param name="status">The payment status.</param>
    private PaymentStatus(string status) : base(PaymentStatusId.Create())
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
