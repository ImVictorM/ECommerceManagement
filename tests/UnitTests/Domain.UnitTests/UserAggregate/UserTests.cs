using Domain.Common.Errors;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using FluentAssertions;

namespace Domain.UnitTests.UserAggregate;

/// <summary>
/// Tests for the <see cref="Domain.UserAggregate.User"/> aggregate root.
/// </summary>
public class UserTests
{
    /// <summary>
    /// List of hash and salt invalid pairs.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> HashSaltInvalidPairs()
    {
        yield return new object[] { "invalid_hash", TestConstants.User.PasswordSalt };
        yield return new object[] { TestConstants.User.PasswordHash, "invalid_salt" };
        yield return new object[] { "invalid_hash", "invalid_salt" };
        yield return new object[] { "", "" };
    }

    /// <summary>
    /// Tests if it creates a brand new user instance correctly.
    /// </summary>
    [Fact]
    public void User_WhenUserCredentialsAreValid_CreatesNewActiveInstance()
    {
        var user = UserUtils.CreateUser();

        user.Should().NotBeNull();
        user.IsActive.Should().BeTrue();
        user.UserRoles.Count.Should().Be(1);
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
    /// Tests if it is possible to associate a user with a role id.
    /// </summary>
    [Fact]
    public void User_WhenAssociatingRoleIdWithUser_IncreasesTheUserRolesCount()
    {
        var roleIdToAdd = 10;
        var user = UserUtils.CreateUser();

        user.AddUserRole(RoleUtils.CreateRoleId(roleIdToAdd));

        user.UserRoles.Count.Should().Be(2);

        user.UserRoles.Select(ur => ur.RoleId.Value).Should().Contain(roleIdToAdd);
    }

    /// <summary>
    /// Test if adding the same role id ignores it.
    /// </summary>
    [Fact]
    public void User_WhenAddingDuplicateRoleId_IgnoresIt()
    {
        var roleIdToAdd = 7;
        var user = UserUtils.CreateUser();

        user.AddUserRole(RoleUtils.CreateRoleId(roleIdToAdd));

        user.UserRoles.Count.Should().Be(2);

        user.AddUserRole(RoleUtils.CreateRoleId(roleIdToAdd));

        user.UserRoles.Count.Should().Be(2);
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
}
