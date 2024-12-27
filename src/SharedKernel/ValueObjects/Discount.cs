using SharedKernel.Errors;
using SharedKernel.Models;

namespace SharedKernel.ValueObjects;

/// <summary>
/// Represents a discount.
/// </summary>
public class Discount : ValueObject
{
    /// <summary>
    /// Gets the discount description;
    /// </summary>
    public string Description { get; } = null!;
    /// <summary>
    /// Gets the discount percentage.
    /// </summary>
    public Percentage Percentage { get; } = null!;
    /// <summary>
    /// Gets the discount starting date.
    /// </summary>
    public DateTimeOffset StartingDate { get; }
    /// <summary>
    /// Gets the discount ending date.
    /// </summary>
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

    private Discount() { }

    private Discount(
        Percentage percentage,
        string description,
        DateTimeOffset startingDate,
        DateTimeOffset endingDate
    )
    {
        Percentage = percentage;
        Description = description;
        StartingDate = startingDate;
        EndingDate = endingDate;

        ValidateDiscount();
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
        Percentage percentage,
        string description,
        DateTimeOffset startingDate,
        DateTimeOffset endingDate
    )
    {
        return new Discount(percentage, description, startingDate, endingDate);
    }

    private void ValidateDiscount()
    {
        var now = DateTimeOffset.UtcNow;
        var isDateRangeValid = StartingDate > now.AddDays(-1) && EndingDate > StartingDate.AddHours(1);
        var isPercentageRangeValid = Percentage.IsWithinRange(1, 100);

        if (!isDateRangeValid)
        {
            throw new DomainValidationException("The date range between the starting and ending date is invalid");
        }

        if (!isPercentageRangeValid)
        {
            throw new DomainValidationException("Discount percentage must be between 1 and 100");
        }
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Percentage;
        yield return Description;
        yield return StartingDate;
        yield return EndingDate;
    }
}
