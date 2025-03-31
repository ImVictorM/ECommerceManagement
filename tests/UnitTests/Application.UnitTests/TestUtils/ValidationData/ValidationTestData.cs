namespace Application.UnitTests.TestUtils.ValidationData;

/// <summary>
/// Defines validation test data for common validations.
/// </summary>
public static partial class ValidationTestData
{
    /// <summary>
    /// Provides a list of empty strings.
    /// </summary>
    public static IEnumerable<object[]> EmptyStrings =>
    [
        [""],
        ["     "],
    ];

    /// <summary>
    /// Provides a list of non-positive numbers, including zero.
    /// </summary>
    public static IEnumerable<object[]> NonPositiveNumbers =>
    [
        [-100],
        [0],
        [-1]
    ];

    /// <summary>
    /// Provides a list of negative numbers.
    /// </summary>
    public static IEnumerable<object[]> NegativeNumbers =>
    [
        [-100],
        [-500],
        [-1]
    ];
}
