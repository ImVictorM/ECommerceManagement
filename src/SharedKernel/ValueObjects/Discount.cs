using SharedKernel.Errors;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.Specifications;

namespace SharedKernel.ValueObjects;

/// <summary>
/// Represents a discount.
/// </summary>
public class Discount : ValueObject, IDiscount
{
    /// <inheritdoc/>
    public string Description { get; } = null!;
    /// <inheritdoc/>
    public int Percentage { get; }
    /// <inheritdoc/>
    public DateTimeOffset StartingDate { get; }
    /// <inheritdoc/>
    public DateTimeOffset EndingDate { get; }

    /// <summary>
    /// Gets a boolean value indicating if the discount is valid to date.
    /// </summary>
    public bool IsValidToDate
    {
        get
        {
            var now = DateTimeOffset.UtcNow;

            return now >= StartingDate && now <= EndingDate;
        }
    }

    public bool IsActive { get; }

    private Discount() { }

    private Discount(
        int percentage,
        string description,
        DateTimeOffset startingDate,
        DateTimeOffset endingDate
    )
    {
        Percentage = percentage;
        Description = description;
        StartingDate = startingDate;
        EndingDate = endingDate;
        IsActive = true;

        if (!new DiscountDateRangeSpecification().IsSatisfiedBy(this))
        {
            throw new DomainValidationException("The date range between the starting and ending date is incorrect");
        }

        if (!new DiscountPercentageRangeSpecification().IsSatisfiedBy(this))
        {
            throw new DomainValidationException("Discount percentage must be between 1 and 100");
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Discount"/> class.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    /// <param name="description">The discount description.</param>
    /// <param name="startingDate">The discount starting date.</param>
    /// <param name="endingDate">The discount ending date.</param>
    /// <returns>A new instance of the <see cref="Discount"/> class.</returns>
    public static Discount Create(
        int percentage,
        string description,
        DateTimeOffset startingDate,
        DateTimeOffset endingDate
    )
    {
        return new Discount(percentage, description, startingDate, endingDate);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Percentage;
        yield return Description;
        yield return StartingDate;
        yield return EndingDate;
        yield return IsActive;
    }
}
