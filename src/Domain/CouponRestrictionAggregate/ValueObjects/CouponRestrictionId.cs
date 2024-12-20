using SharedKernel.Errors;
using SharedKernel.Models;
using System.Globalization;

namespace Domain.CouponRestrictionAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for a <see cref="CouponRestriction"/> restriction.
/// </summary>
public class CouponRestrictionId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of <see cref="CouponRestrictionId"/> class.
    /// </summary>
    private CouponRestrictionId() { }

    /// <summary>
    /// Initiates a new instance of <see cref="CouponRestrictionId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private CouponRestrictionId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of <see cref="CouponRestrictionId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="CouponRestrictionId"/> class with the specified identifier.</returns>
    public static CouponRestrictionId Create(long value)
    {
        return new CouponRestrictionId(value);
    }

    /// <summary>
    /// Creates a new instance of <see cref="CouponRestrictionId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of <see cref="CouponRestrictionId"/> class with the specified identifier.</returns>
    public static CouponRestrictionId Create(string value)
    {
        if (long.TryParse(value, out var id))
        {
            return new CouponRestrictionId(id);
        }

        throw new BaseException(
            message: "There was an error when converting the coupon restriction id.",
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
