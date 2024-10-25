using Domain.Common.Errors;
using Domain.Common.Models;
using Domain.PaymentAggregate.ValueObjects;
using SharedResources.Extensions;

namespace Domain.PaymentAggregate.Entities;

/// <summary>
/// Represents the payment.
/// </summary>
public sealed class PaymentStatus : Entity<PaymentStatusId>
{
    /// <summary>
    /// Represents a pending status.
    /// </summary>
    public static readonly PaymentStatus Pending = new(PaymentStatusId.Create(1), nameof(Pending).ToLowerSnakeCase());
    /// <summary>
    /// Represents a completed status.
    /// </summary>
    public static readonly PaymentStatus Completed = new(PaymentStatusId.Create(2), nameof(Completed).ToLowerSnakeCase());
    /// <summary>
    /// Represents a failed status.
    /// </summary>
    public static readonly PaymentStatus Failed = new(PaymentStatusId.Create(3), nameof(Failed).ToLowerSnakeCase());
    /// <summary>
    /// Represents a refunded status.
    /// </summary>
    public static readonly PaymentStatus Refunded = new(PaymentStatusId.Create(4), nameof(Refunded).ToLowerSnakeCase());

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
    /// <param name="id">The status identifier.</param>
    /// <param name="name">The status name.</param>
    private PaymentStatus(PaymentStatusId id, string name) : base(id)
    {
        Name = name;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatus"/> class.
    /// </summary>
    /// <param name="name">The payment status.</param>
    /// <returns>A new instance of the <see cref="PaymentStatus"/> class.</returns>
    public static PaymentStatus Create(string name)
    {
        return GetPaymentStatusByName(name) ?? throw new DomainValidationException($"The {name} payment status does not exist");
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
}
