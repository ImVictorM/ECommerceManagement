
using Domain.ProductReviewAggregate.Errors;

using SharedKernel.Models;

namespace Domain.ProductReviewAggregate.ValueObjects;

/// <summary>
/// Represents a star rating.
/// </summary>
public class StarRating : ValueObject
{
    private const int MinRating = 0;
    private const int MaxRating = 5;

    /// <summary>
    /// Gets the star rating value.
    /// </summary>
    public int Value { get; }

    private StarRating() { }

    private StarRating(int value)
    {
        var isRatingValid = IsRatingInValidRange(value);

        if (!isRatingValid)
        {
            throw new InvalidStarRatingRangeException();
        }

        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="StarRating"/> class.
    /// </summary>
    /// <param name="value">
    /// The rating value. Must be between 0 and 5 inclusive.
    /// </param>
    /// <returns>A new instance of the <see cref="StarRating"/> class.</returns>
    public static StarRating Create(int value)
    {
        return new StarRating(value);
    }

    private static bool IsRatingInValidRange(int value)
    {
        return value >= MinRating && value <= MaxRating;
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
