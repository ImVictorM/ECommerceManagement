using Domain.Common.Errors;
using Domain.Common.Models;

namespace Domain.Common.ValueObjects;

/// <summary>
/// Represents a discount in percentage with
/// starting and ending date.
/// </summary>
public sealed class Discount : ValueObject
{
    /// <summary>
    /// Gets the discount percentage.
    /// </summary>
    public int Percentage { get; }
    /// <summary>
    /// Gets the discount description.
    /// </summary>
    public string Description { get; } = string.Empty;
    /// <summary>
    /// Gets the discount starting date.
    /// </summary>
    public DateTimeOffset StartingDate { get; }
    /// <summary>
    /// Gets the discount ending date.
    /// </summary>
    public DateTimeOffset EndingDate { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="Discount"/> class.
    /// </summary>
    private Discount() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="Discount"/> class.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    /// <param name="description">The discount description.</param>
    /// <param name="startingDate">The discount starting date.</param>
    /// <param name="endingDate">The discount ending date.</param>
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

        Validate();
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

    /// <summary>
    /// Validate the address fields.
    /// </summary>
    /// <exception cref="DomainValidationException">Exception thrown case any field is invalid.</exception>
    private void Validate()
    {
        var now = DateTimeOffset.UtcNow;

        if (StartingDate < now.AddDays(1))
        {
            throw new DomainValidationException("The starting date for the discount must be at least one day in the future");
        }
        else if (EndingDate < StartingDate.AddHours(1))
        {
            throw new DomainValidationException("The ending date and time must be at least one hour after the starting date");
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
