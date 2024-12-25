using Domain.UserAggregate.ValueObjects;
using SharedKernel.Authorization;
using SharedKernel.UnitTests.TestUtils.Constants;

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
        public const string Email = SharedKernelConstants.Email.Value;
        /// <summary>
        /// The user roles constant.
        /// </summary>
        public static readonly IReadOnlySet<Role> UserRoles = new HashSet<Role>()
        {
            Role.Customer
        };
    }
}
