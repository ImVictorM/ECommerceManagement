using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;

using SharedKernel.Authorization;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

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
            EmailUtils.CreateEmail(),
            PasswordHashUtils.Create(),
            DomainConstants.User.Phone,
        };

        yield return new object[] {
            "Djhon djhones",
            EmailUtils.CreateEmail(),
            PasswordHashUtils.Create(),
            DomainConstants.User.Phone,
        };

        yield return new object[] {
            DomainConstants.User.Name,
            EmailUtils.CreateEmail(),
            PasswordHashUtils.Create(),
            "19987093231",
        };
    }

    /// <summary>
    /// Tests if it creates a brand new user instance correctly.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="passwordHash">The user password hash.</param>
    /// <param name="phone">The user phone.</param>
    /// <param name="email">The user email.</param>
    [Theory]
    [MemberData(nameof(ValidUserParameters))]
    public void User_WhenUserCredentialsAreValid_CreatesNewActiveCustomer(
        string name,
        Email email,
        PasswordHash passwordHash,
        string? phone
    )
    {
        var user = UserUtils.CreateUser(
            name: name,
            passwordHash: passwordHash,
            phone: phone,
            email: email
        );

        user.Should().NotBeNull();
        user.Name.Should().Be(name);
        user.PasswordHash.Should().BeEquivalentTo(passwordHash);
        user.Phone.Should().Be(phone);
        user.Email.Should().BeEquivalentTo(email);
        user.IsActive.Should().BeTrue();
        user.UserRoles.Count.Should().Be(1);
        user.UserRoles.Should().ContainSingle(ur => ur.RoleId == Role.Customer.Id);
        user.UserAddresses.Count.Should().Be(0);
    }

    /// <summary>
    /// Test if it is possible to make the user inactive.
    /// </summary>
    [Fact]
    public void User_WhenMakingTheUserInactive_TheActiveFieldIsSetToFalse()
    {
        var user = UserUtils.CreateUser();

        user.Deactivate();

        user.IsActive.Should().BeFalse();
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
    public void User_WhenGettingUserRoleNames_ReturnsItCorrectly()
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

        user.AssignAddress(address);

        user.UserAddresses.Count.Should().Be(1);
        user.UserAddresses.Should().Contain(address);
    }
}
