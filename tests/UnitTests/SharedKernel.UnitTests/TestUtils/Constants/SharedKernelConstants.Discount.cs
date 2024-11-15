namespace Domain.UnitTests.TestUtils.Constants;

public static partial class SharedKernelConstants
{
    /// <summary>
    /// Declare constants related to the <see cref="SharedKernel.ValueObjects.Discount"/>.
    /// </summary>
    public static class Discount
    {
        /// <summary>
        /// The discount percentage constant.
        /// </summary>
        public const int Percentage = 10;
        /// <summary>
        /// The discount description constant.
        /// </summary>
        public const string Description = "Testing discount";
        /// <summary>
        /// The discount starting date constant.
        /// </summary>
        public static readonly DateTimeOffset StartingDate = DateTimeOffset.UtcNow;
        /// <summary>
        /// The discount ending date constant.
        /// </summary>
        public static readonly DateTimeOffset EndingDate = StartingDate.AddDays(5);

        /// <summary>
        /// Creates a new unique description based on an index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A new unique description.</returns>
        public static string CreateDescriptionFromIndex(int index)
        {
            return $"{Description}-{index}";
        }
    }
}
