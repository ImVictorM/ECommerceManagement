using System.Globalization;
using SharedKernel.Errors;
using SharedKernel.Models;

namespace Domain.CouponAggregate.ValueObjects;

/// <summary>
/// Represents a coupon identifier.
/// </summary>
public sealed class CouponId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of <see cref="CouponId"/> class.
    /// </summary>
    private CouponId() { }

    /// <summary>
    /// Initiates a new instance of <see cref="CouponId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private CouponId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="CouponId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="CouponId"/> class with the specified identifier.</returns>
    public static CouponId Create(long value)
    {
        return new CouponId(value);
    }

    /// <summary>
    /// Creates a new instance of <see cref="CouponId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="CouponId"/> class with the specified identifier.</returns>
    public static CouponId Create(string value)
    {
        if (long.TryParse(value, out var id))
        {
            return new CouponId(id);
        }

        throw new BaseException(
            message: "There was an error when converting the coupon id.",
            errorCode: ErrorCode.InvalidOperation,
            title: "Domain Error - Invalid Operation"
        );
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
