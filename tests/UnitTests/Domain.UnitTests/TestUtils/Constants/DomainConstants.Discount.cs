namespace Domain.UnitTests.TestUtils.Constants;

public static partial class DomainConstants
{
    /// <summary>
    /// Declare constants related to the <see cref="Domain.Common.ValueObjects.Discount"/> aggregate root for testing purposes.
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
        public static readonly DateTimeOffset StartingDate = DateTimeOffset.Now.AddDays(10);
        /// <summary>
        /// The discount ending date constant.
        /// </summary>
        public static readonly DateTimeOffset EndingDate = StartingDate.AddDays(5);
    }
}
