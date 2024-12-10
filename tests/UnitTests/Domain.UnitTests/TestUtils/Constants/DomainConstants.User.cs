using Domain.UserAggregate.ValueObjects;
using SharedKernel.Authorization;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils.Constants;

public static partial class DomainConstants
{
    /// <summary>
    /// Define the <see cref="Domain.UserAggregate.User"/> test constants.
    /// </summary>
    public static class User
    {
        /// <summary>
        /// The user identifier constant.
        /// </summary>
        public static readonly UserId Id = UserId.Create(1);
        /// <summary>
        /// The user name constant.
        /// </summary>
        public const string Name = "Mjoln the Gold-seeker";
        /// <summary>
        /// The user phone constant.
        /// </summary>
        public const string Phone = "19999999999";
        /// <summary>
        /// The user password.
        /// </summary>
        public const string Password = "user123";
        /// <summary>
        /// The user password hash constant.
        /// </summary>
        public const string PasswordHash = "B7826B04FFB59161987736D585801B5FD59C131F6088C04A1CF8C02C2027F12E";
        /// <summary>
        /// The user password salt constant.
        /// </summary>
        public const string PasswordSalt = "D4583B5F6790FB289897C2223F0483EA";
        /// <summary>
        /// The user email constant.
        /// </summary>
        public static readonly Email Email = EmailUtils.CreateEmail();
        /// <summary>
        /// The user role constant.
        /// </summary>
        public static readonly Role Role = Role.Customer;

        /// <summary>
        /// Returns a name concatenated with an index.
        /// </summary>
        /// <param name="index">The index to concatenate with.</param>
        /// <returns>A new name with a concatenated index.</returns>
        public static string UserNameFromIndex(int index) => $"{Name}-{index}";

        /// <summary>
        /// Defines user validation error message constants.
        /// </summary>
        public static class Validations
        {
            /// <summary>
            /// Error message for short names.
            /// </summary>
            public const string ShortName = "'Name' must be at least 3 characters long.";
            /// <summary>
            /// Error message for empty names.
            /// </summary>
            public const string EmptyName = "'Name' must not be empty.";

            /// <summary>
            /// Error message for empty passwords.
            /// </summary>
            public const string EmptyPassword = "'Password' must not be empty.";
            /// <summary>
            /// Error message for short passwords.
            /// </summary>
            public const string ShortPassword = "'Password' must be at least 6 characters long.";
            /// <summary>
            /// Error message for passwords missing a digit.
            /// </summary>
            public const string MissingDigitPassword = "'Password' must contain at least one digit.";
            /// <summary>
            /// Error message for passwords missing a character.
            /// </summary>
            public const string MissingCharacterPassword = "'Password' must contain at least one character.";
        }
    }
}
