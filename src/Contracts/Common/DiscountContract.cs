namespace Contracts.Common;

/// <summary>
/// Represents a discount request/response.
/// </summary>
/// <param name="Percentage">The discount percentage.</param>
/// <param name="Description">The discount description.</param>
/// <param name="StartingDate">The discount starting date.</param>
/// <param name="EndingDate">The discount ending date.</param>
public record DiscountContract(
    int Percentage,
    string Description,
    DateTimeOffset StartingDate,
    DateTimeOffset EndingDate
);
