namespace Application.UnitTests.TestUtils.ValidationData;

/// <summary>
/// Defines validation test data for common validations.
/// </summary>
public static partial class ValidationTestData
{
    /// <summary>
    /// List of empty strings.
    /// </summary>
    public static IEnumerable<object[]> EmptyStrings =>
    [
        [""],
        ["     "],
    ];

    /// <summary>
    /// List of non-positive numbers, including zero.
    /// </summary>
    public static IEnumerable<object[]> NonPositiveNumbers =>
    [
        [-100],
        [0],
        [-1]
    ];
}
