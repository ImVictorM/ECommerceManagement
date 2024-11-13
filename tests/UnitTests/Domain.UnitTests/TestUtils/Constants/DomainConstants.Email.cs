namespace Domain.UnitTests.TestUtils.Constants;

public static partial class DomainConstants
{
    /// <summary>
    /// Define the <see cref="SharedKernel.ValueObjects.Email"/> test constants.
    /// </summary>
    public static class Email
    {
        /// <summary>
        /// Gets a valid email value.
        /// </summary>
        public const string Value = "maya_lives@email.com";

        /// <summary>
        /// Returns an email concatenated with an index.
        /// </summary>
        /// <param name="index">The index to concatenate with.</param>
        /// <returns>A new email with a concatenated index.</returns>
        public static string EmailFromIndex(int index) => $"{index}{Value}";

        /// <summary>
        /// Defines validation error message constants.
        /// </summary>
        public static class Validations
        {
            /// <summary>
            /// Invalid email pattern constant error message.
            /// </summary>
            public const string InvalidPatternEmail = "'Email' does not follow the required pattern.";
            /// <summary>
            /// Empty email constant error message.
            /// </summary>
            public const string EmptyEmail = "'Email' must not be empty.";
        }
    }
}
