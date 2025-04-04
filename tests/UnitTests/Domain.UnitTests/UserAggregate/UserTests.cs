using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace Domain.UnitTests.UserAggregate;

/// <summary>
/// Unit tests for the <see cref="User"/> class.
/// </summary>
public class UserTests
{
    /// <summary>
    /// Provides a list of valid parameters to create new users.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidUserCreationParameters =
    [
        [
            UserUtils.CreateUserName(),
            EmailUtils.CreateEmail(),
            PasswordHashUtils.Create(),
            "10495837582"
        ],
        [
            UserUtils.CreateUserName(),
            EmailUtils.CreateEmail(),
            PasswordHashUtils.Create(),
        ],
    ];

    /// <summary>
    /// Provides a list of users containing the expected return value when calling
    /// the <see cref="User.IsAdmin"/> method.
    /// </summary>
    public static readonly IEnumerable<object[]> UsersAndExpectedIsAdminReturnValue =
    [
        [
           UserUtils.CreateCustomer(),
            false
        ],
        [
            UserUtils.CreateAdmin(),
            true
        ],
    ];

    /// <summary>
    /// Verifies a customer user is created correctly.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidUserCreationParameters))]
    public void CreateCustomer_WithValidParameters_CreatesWithoutThrowing(
        string name,
        Email email,
        PasswordHash passwordHash,
        string? phone = null
    )
    {
        var actionResult = FluentActions
            .Invoking(() => User.CreateCustomer(
                name: name,
                email: email,
                passwordHash: passwordHash,
                phone: phone
            ))
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
    /// Verifies an admin user is created correctly.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidUserCreationParameters))]
    public void CreateAdmin_WithValidParameters_CreatesWithoutThrowing(
        string name,
        Email email,
        PasswordHash passwordHash,
        string? phone = null
    )
    {
        var actionResult = FluentActions
            .Invoking(() => User.CreateAdmin(
                name: name,
                email: email,
                passwordHash: passwordHash,
                phone: phone
            ))
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
    public void Deactivate_WithActiveUser_DeactivatesCorrectly()
    {
        var user = UserUtils.CreateCustomer();

        user.Deactivate();

        user.IsActive.Should().BeFalse();
    }

    /// <summary>
    /// Verifies if it is possible to add a new address to a user.
    /// </summary>
    [Fact]
    public void AssignAddress_WithValidAddress_AddsTheAddress()
    {
        var user = UserUtils.CreateCustomer();
        var address = AddressUtils.CreateAddress();

        user.AssignAddress(address);

        user.UserAddresses.Count.Should().Be(1);
        user.UserAddresses.Should().Contain(address);
    }

    /// <summary>
    /// Verifies the <see cref="User.Update"/> method updates the
    /// user data correctly.
    /// </summary>
    [Fact]
    public void Update_WithValidParameters_UpdatesCorrectly()
    {
        var user = UserUtils.CreateCustomer();

        var newUserName = "Roberto Carlos";
        var newUserPhone = "199485924738";
        var newUserEmail = EmailUtils.CreateEmail("new_email@email.com");

        user.Update(name: newUserName, phone: newUserPhone, email: newUserEmail);

        user.Name.Should().Be(newUserName);
        user.Phone.Should().Be(newUserPhone);
        user.Email.Should().Be(newUserEmail);
    }

    /// <summary>
    /// Verifies the <see cref="User.IsAdmin"/> method returns the correct value.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="expectedIsAdminReturnValue">The expected response.</param>
    [Theory]
    [MemberData(nameof(UsersAndExpectedIsAdminReturnValue))]
    public void IsAdmin_WithDifferentUsers_ReturnsExpectedBoolean(
        User user,
        bool expectedIsAdminReturnValue
    )
    {
        user.IsAdmin().Should().Be(expectedIsAdminReturnValue);
    }
}
