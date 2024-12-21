using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.PaymentAggregate.Enumeration;

/// <summary>
/// Represents the payment.
/// </summary>
public sealed class PaymentStatus : BaseEnumeration
{
    /// <summary>
    /// Represents an in progress status.
    /// </summary>
    public static readonly PaymentStatus InProgress = new(1, nameof(InProgress).ToLowerSnakeCase());
    /// <summary>
    /// Represents a pending status.
    /// </summary>
    public static readonly PaymentStatus Pending = new(2, nameof(Pending).ToLowerSnakeCase());
    /// <summary>
    /// Represents an approved status.
    /// </summary>
    public static readonly PaymentStatus Approved = new(3, nameof(Approved).ToLowerSnakeCase());
    /// <summary>
    /// Represents a rejected status.
    /// </summary>
    public static readonly PaymentStatus Rejected = new(4, nameof(Rejected).ToLowerSnakeCase());
    /// <summary>
    /// Represents a canceled status.
    /// </summary>
    public static readonly PaymentStatus Canceled = new(5, nameof(Canceled).ToLowerSnakeCase());
    /// <summary>
    /// Represents a refunded status.
    /// </summary>
    public static readonly PaymentStatus Refunded = new(6, nameof(Refunded).ToLowerSnakeCase());

    private PaymentStatus() { }

    private PaymentStatus(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Gets all the payment statuses in a list format.
    /// </summary>
    /// <returns>All the payment statuses.</returns>
    public static IEnumerable<PaymentStatus> List()
    {
        return GetAll<PaymentStatus>();
    }
}
