using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.PaymentAggregate.Enumerations;

/// <summary>
/// Represents a payment status.
/// </summary>
public class PaymentStatus : BaseEnumeration
{
    /// <summary>
    /// Represents a pending payment.
    /// </summary>
    public static readonly PaymentStatus Pending = new(1, nameof(Pending).ToLowerSnakeCase());
    /// <summary>
    /// Represents a payment in progress.
    /// </summary>
    public static readonly PaymentStatus InProgress = new(2, nameof(InProgress).ToLowerSnakeCase());
    /// <summary>
    /// Represents an authorized payment.
    /// </summary>
    public static readonly PaymentStatus Authorized = new(3, nameof(Authorized).ToLowerSnakeCase());
    /// <summary>
    /// Represents an approved payment.
    /// </summary>
    public static readonly PaymentStatus Approved = new(4, nameof(Approved).ToLowerSnakeCase());
    /// <summary>
    /// Represents an rejected payment.
    /// </summary>
    public static readonly PaymentStatus Rejected = new(5, nameof(Rejected).ToLowerSnakeCase());
    /// <summary>
    /// Represents an canceled payment.
    /// </summary>
    public static readonly PaymentStatus Canceled = new(6, nameof(Canceled).ToLowerSnakeCase());
    /// <summary>
    /// Represents a refunded payment.
    /// </summary>
    public static readonly PaymentStatus Refunded = new(7, nameof(Refunded).ToLowerSnakeCase());

    private PaymentStatus(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// List all the available payment statuses.
    /// </summary>
    /// <returns>All the available payment statuses.</returns>
    public static IReadOnlyList<PaymentStatus> List()
    {
        return GetAll<PaymentStatus>().ToList().AsReadOnly();
    }
}
