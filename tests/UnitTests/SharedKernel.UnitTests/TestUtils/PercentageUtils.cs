using SharedKernel.UnitTests.TestUtils.Constants;
using SharedKernel.ValueObjects;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Percentage"/> class.
/// </summary>
public static class PercentageUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Percentage"/> class.
    /// </summary>
    /// <param name="value">The percentage value.</param>
    /// <returns>A new instance of the <see cref="Percentage"/> class.</returns>
    public static Percentage Create(int value = SharedKernelConstants.Percentage.Value)
    {
        return Percentage.Create(value);
    }
}
