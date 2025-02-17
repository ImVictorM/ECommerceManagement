using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.UserAggregate;

/// <summary>
/// Unit tests for the <see cref="User"/> class.
/// </summary>
public class UserTests
{
    /// <summary>
    /// List of valid action to create new users.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidActionsToCreateUser =
    [
        [
            () => UserUtils.CreateUser(name: "New user name"),
        ],
        [
            () => UserUtils.CreateUser(roles: new HashSet<UserRole>() { UserRole.Create(4) }),
        ],
        [
            () => UserUtils.CreateUser(phone: "19987093231"),
        ],
    ];

    /// <summary>
    /// Tests an user is created correctly.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidActionsToCreateUser))]
    public void CreateUser_WithValidParameters_CreatesWithoutThrowing(
        Func<User> action
    )
    {
        var actionResult = FluentActions
            .Invoking(action)
            .Should()
            .NotThrow();

        var user = actionResult.Subject;

        user.Should().NotBeNull();
        user.Name.Should().NotBeNullOrWhiteSpace();
        user.PasswordHash.Should().NotBeNull();
        user.Email.Should().NotBeNull();
        user.IsActive.Should().BeTrue();
        user.UserRoles.Should().NotBeNullOrEmpty();
        user.UserAddresses.Count.Should().Be(0);
    }

    /// <summary>
    /// Test if it is possible to make the user inactive.
    /// </summary>
    [Fact]
    public void DeactivateUser_WhenCallingDeactivateMethod_TheActiveFieldIsSetToFalse()
    {
        var user = UserUtils.CreateUser();

        user.Deactivate();

        user.IsActive.Should().BeFalse();
    }

    /// <summary>
    /// Tests if it is possible to add roles to a current user.
    /// </summary>
    [Fact]
    public void AssignRole_WhenAssigningNewRole_IncrementsUserRoles()
    {
        var user = UserUtils.CreateUser(roles: new HashSet<UserRole>()
        {
            UserRole.Create(1),
        });
        var userNewRole = UserRole.Create(2);

        user.AssignRole(userNewRole);

        user.UserRoles.Count.Should().Be(2);

        user.UserRoles.Should().Contain(userNewRole);
    }

    /// <summary>
    /// Tests if it is possible to add a new address to a user.
    /// </summary>
    [Fact]
    public void AssignAddress_WithValidAddress_AddsTheAddress()
    {
        var user = UserUtils.CreateUser();
        var address = AddressUtils.CreateAddress();

        user.AssignAddress(address);

        user.UserAddresses.Count.Should().Be(1);
        user.UserAddresses.Should().Contain(address);
    }

    /// <summary>
    /// Tests the <see cref="User.Update(string?, string?, SharedKernel.ValueObjects.Email?)"/> method updates the
    /// user data correctly.
    /// </summary>
    [Fact]
    public void UpdateUser_WhenCalledWithValidParameters_UpdatesTheUserData()
    {
        var user = UserUtils.CreateUser();

        var newUserName = "Roberto Carlos";
        var newUserPhone = "199485924738";
        var newUserEmail = EmailUtils.CreateEmail("new_email@email.com");

        user.Update(name: newUserName, phone: newUserPhone, email: newUserEmail);

        user.Name.Should().Be(newUserName);
        user.Phone.Should().Be(newUserPhone);
        user.Email.Should().Be(newUserEmail);
    }
}
