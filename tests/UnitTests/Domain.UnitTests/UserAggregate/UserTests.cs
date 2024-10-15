using Domain.Common.Errors;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.UserAggregate;

/// <summary>
/// Tests for the <see cref="Domain.UserAggregate.User"/> aggregate root.
/// </summary>
public class UserTests
{
    /// <summary>
    /// List of valid user parameters to create a new user.
    /// </summary>
    /// <returns>A list of user parameters.</returns>
    public static IEnumerable<object[]> ValidUserParameters()
    {
        yield return new object[] {
            DomainConstants.User.Name,
            DomainConstants.User.PasswordHash,
            DomainConstants.User.PasswordSalt,
            DomainConstants.User.Phone,
            DomainConstants.Email.Value,
            Role.Customer
        };
    }
    /// <summary>
    /// List of hash and salt invalid pairs.
    /// </summary>
    /// <returns>A list of hash and salt invalid pairs.</returns>
    public static IEnumerable<object[]> HashSaltInvalidPairs()
    {
        yield return new object[] { "invalid_hash", DomainConstants.User.PasswordSalt };
        yield return new object[] { DomainConstants.User.PasswordHash, "invalid_salt" };
        yield return new object[] { "invalid_hash", "invalid_salt" };
        yield return new object[] { "", "" };
    }

    /// <summary>
    /// Tests if it creates a brand new user instance correctly.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="passwordHash">The user password hash.</param>
    /// <param name="passwordSalt">The user password salt.</param>
    /// <param name="phone">The user phone.</param>
    /// <param name="email">The user email.</param>
    /// <param name="role">The user role.</param>
    [Theory]
    [MemberData(nameof(ValidUserParameters))]
    public void User_WhenUserCredentialsAreValid_CreatesNewActiveInstance(
        string name,
        string passwordHash,
        string passwordSalt,
        string phone,
        string email,
        Role role
    )
    {
        var user = UserUtils.CreateUser(
            name: name,
            passwordHash: passwordHash,
            passwordSalt: passwordSalt,
            phone: phone,
            email: email
        );

        var expectedPasswordHash = $"{passwordHash}-{passwordSalt}";

        user.Should().NotBeNull();
        user.Name.Should().Be(name);
        user.PasswordHash.Value.Should().Be(expectedPasswordHash);
        user.Phone.Should().Be(phone);
        user.Email.Value.Should().Be(email);
        user.IsActive.Should().BeTrue();
        user.UserRoles.Count.Should().Be(1);
        user.UserRoles.Select(ur => ur.Role).Should().Contain(role);
        user.UserAddresses.Count.Should().Be(0);
    }

    /// <summary>
    /// Tests if invalid pairs of hash and salt generates an error.
    /// </summary>
    /// <param name="hash">The password hash.</param>
    /// <param name="salt">The password salt.</param>
    [Theory]
    [MemberData(nameof(HashSaltInvalidPairs))]
    public void User_WhenPasswordHashOrSaltAreNotHexadecimalValues_GeneratesAnError(string hash, string salt)
    {
        Action act = () => UserUtils.CreateUser(passwordHash: hash, passwordSalt: salt);

        act.Should().Throw<DomainValidationException>().WithMessage("The hash or salt is not in a valid hexadecimal format");
    }

    /// <summary>
    /// Test if it is possible to make the user inactive.
    /// </summary>
    [Fact]
    public void User_WhenMakingTheUserInactive_TheActiveFieldIsSetToFalse()
    {
        var user = UserUtils.CreateUser();

        user.MakeInactive();

        user.IsActive.Should().BeFalse();
    }

    /// <summary>
    /// Tests if it is possible to add roles to a current user.
    /// </summary>
    [Fact]
    public void User_WhenAddingRole_AddAndIncrementsTheUserRolesCount()
    {
        var user = UserUtils.CreateUser();

        user.AddUserRole(Role.Admin);

        user.UserRoles.Count.Should().Be(2);
        user.UserRoles.Select(ur => ur.Role).Should().Contain(Role.Admin);
    }
}
