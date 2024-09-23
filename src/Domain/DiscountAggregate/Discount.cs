using Domain.Common.Models;
using Domain.DiscountAggregate.ValueObjects;

namespace Domain.DiscountAggregate;

/// <summary>
/// Represents a discount in percentage with
/// starting and ending date.
/// </summary>
public sealed class Discount : Entity<DiscountId>
{
    /// <summary>
    /// Gets the discount percentage.
    /// </summary>
    public int Percentage { get; private set; }
    /// <summary>
    /// Gets the discount description.
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// Gets the discount starting date.
    /// </summary>
    public DateTimeOffset StartingDate { get; private set; }
    /// <summary>
    /// Gets the discount ending date.
    /// </summary>
    public DateTimeOffset EndingDate { get; private set; }

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
    ) : base(DiscountId.Create())
    {
        Percentage = percentage;
        Description = description;
        StartingDate = startingDate;
        EndingDate = endingDate;
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
}
