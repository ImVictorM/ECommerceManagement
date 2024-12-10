namespace Domain.UnitTests.TestUtils.Constants;

public static partial class SharedKernelConstants
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
    }
}
