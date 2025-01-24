namespace Application.UnitTests.TestUtils.ValidationData;

public static partial class ValidationTestData
{
    /// <summary>
    /// Common validations for emails.
    /// </summary>
    public static class Email
    {
        private const string EmailEmptyErrorMessage = "'Email' must not be empty.";
        private const string EmailInvalidPatternErrorMessage = "'Email' does not follow the required pattern.";

        /// <summary>
        /// List containing pair of invalid email and expected errors.
        /// </summary>
        public static IEnumerable<object[]> InvalidEmails =>
        [
            [
                "",
                new List<string>()
                {
                    EmailEmptyErrorMessage,
                    EmailInvalidPatternErrorMessage
                }
            ],
            [
                "invalidemailformat",
                new List<string>()
                {
                    EmailInvalidPatternErrorMessage
                }
            ],
            [
                "invalidemailformat@invalid@.com",
                new List<string>()
                {
                    EmailInvalidPatternErrorMessage
                }
            ],
            [
                "invalidemailformat@invalid.com.",
                new List<string>()
                {
                   EmailInvalidPatternErrorMessage
                }
            ]
        ];
    }
}
