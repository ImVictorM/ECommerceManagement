namespace Application.UnitTests.TestUtils.ValidationData;

public static partial class ValidationTestData
{
    /// <summary>
    /// Provides common validations data for user names.
    /// </summary>
    public static class Name
    {
        private const string NameEmptyErrorMessage = "'Name' must not be empty.";
        private const string NameTooShortErrorMessage =
            "'Name' must be at least 3 characters long.";

        /// <summary>
        /// Provides a list containing pairs of invalid names and expected errors.
        /// </summary>
        public static IEnumerable<object[]> InvalidNames =>
        [
            [
                "",
                new List<string>()
                {
                    NameTooShortErrorMessage,
                    NameEmptyErrorMessage,
                }
            ],
            [
                "7S",
                new List<string>()
                {
                    NameTooShortErrorMessage
                }
            ]
        ];
    }
}

