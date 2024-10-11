using Domain.Common.Errors;
using Domain.Common.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents the payment.
/// </summary>
public sealed class PaymentStatus : ValueObject
{
    /// <summary>
    /// Represents a pending status.
    /// </summary>
    public static readonly PaymentStatus Pending = new(nameof(Pending).ToLowerInvariant());
    /// <summary>
    /// Represents a completed status.
    /// </summary>
    public static readonly PaymentStatus Completed = new(nameof(Completed).ToLowerInvariant());
    /// <summary>
    /// Represents a failed status.
    /// </summary>
    public static readonly PaymentStatus Failed = new(nameof(Failed).ToLowerInvariant());
    /// <summary>
    /// Represents a refunded status.
    /// </summary>
    public static readonly PaymentStatus Refunded = new(nameof(Refunded).ToLowerInvariant());

    /// <summary>
    /// Gets the status name.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    private PaymentStatus() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    /// <param name="name">The status name.</param>
    private PaymentStatus(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    /// <param name="name">The payment status.</param>
    /// <returns>A new instance of the <see cref="PaymentStatus"/> class.</returns>
    public static PaymentStatus Create(string name)
    {
        if (GetPaymentStatusByName(name) == null) throw new DomainValidationException($"The {name} payment status does not exist");

        return new PaymentStatus(name);
    }

    /// <summary>
    /// Gets a payment status by name, or null if not found.
    /// </summary>
    /// <param name="name">The payment status name.</param>
    /// <returns>The payment status or null.</returns>
    private static PaymentStatus? GetPaymentStatusByName(string name)
    {
        return List().FirstOrDefault(paymentStatus => paymentStatus.Name == name);
    }

    /// <summary>
    /// Gets all the payment statuses in a list format.
    /// </summary>
    /// <returns>All the payment statuses.</returns>
    public static IEnumerable<PaymentStatus> List()
    {
        return
        [
            Pending,
            Completed,
            Failed,
            Refunded
        ];
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}
