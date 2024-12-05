using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using SharedKernel.Authorization;
using FluentAssertions;
using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;

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
            DomainConstants.User.Email.ToString()
        };

        yield return new object[] {
            "Djhon djhones",
            DomainConstants.User.PasswordHash,
            DomainConstants.User.PasswordSalt,
            DomainConstants.User.Phone,
            DomainConstants.User.Email.ToString()
        };

        yield return new object[] {
            DomainConstants.User.Name,
            "1847564AEFE",
            DomainConstants.User.PasswordSalt,
            DomainConstants.User.Phone,
            DomainConstants.User.Email.ToString()
        };

        yield return new object[] {
            DomainConstants.User.Name,
            DomainConstants.User.PasswordHash,
            "ABCDEF123456",
            DomainConstants.User.Phone,
            DomainConstants.User.Email.ToString()
        };

        yield return new object[] {
            DomainConstants.User.Name,
            DomainConstants.User.PasswordHash,
            DomainConstants.User.PasswordSalt,
            "19987093231",
            DomainConstants.User.Email.ToString()
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
    [Theory]
    [MemberData(nameof(ValidUserParameters))]
    public void User_WhenUserCredentialsAreValid_CreatesNewActiveCustomer(
        string name,
        string passwordHash,
        string passwordSalt,
        string? phone,
        string email
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
        user.UserRoles.Should().ContainSingle(ur => ur.RoleId == Role.Customer.Id);
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
    /// It is possible to create a user with a specific role.
    /// </summary>
    [Fact]
    public void User_WhenCreatingWithSpecificRole_AssociateItCorrectly()
    {
        var user = UserUtils.CreateUser(role: Role.Admin);

        user.UserRoles.Count.Should().Be(1);
        user.UserRoles.Should().ContainSingle(ur => ur.RoleId == Role.Admin.Id);
    }

    /// <summary>
    /// Tests if it is possible to add roles to a current user.
    /// </summary>
    [Fact]
    public void User_WhenAddingRole_AddAndIncrementsTheUserRolesCount()
    {
        var user = UserUtils.CreateUser();

        user.AssignRole(Role.Admin);

        user.UserRoles.Count.Should().Be(2);

        user.UserRoles.Select(ur => ur.RoleId).Should().Contain(Role.Admin.Id);
    }

    /// <summary>
    /// Tests if adding repeated roles ignores it.
    /// </summary>
    [Fact]
    public void User_WhenAddingRepeatedRole_IgnoresIt()
    {
        var user = UserUtils.CreateUser();

        user.AssignRole(Role.Customer);

        user.UserRoles.Count.Should().Be(1);
    }

    /// <summary>
    /// Tests if it is possible to get the user role names.
    /// </summary>
    [Fact]
    public void User_WhenGettingUserRoleNames_RetunsItCorrectly()
    {
        var expectedRoleNames = new string[] { "admin", "customer" };

        var user = UserUtils.CreateUser();

        user.AssignRole(Role.Admin);

        var roleNames = user.GetRoleNames();

        foreach (var roleName in roleNames)
        {
            expectedRoleNames.Should().Contain(roleName);
        }
    }

    /// <summary>
    /// Tests if it is possible to add a new address to a user.
    /// </summary>
    [Fact]
    public void User_WhenAddingAddress_AddsItAndIncreaseCount()
    {
        var user = UserUtils.CreateUser();
        var address = AddressUtils.CreateAddress();

        user.AddAddress(address);

        user.UserAddresses.Count.Should().Be(1);
        user.UserAddresses.Should().Contain(address);
    }
}
