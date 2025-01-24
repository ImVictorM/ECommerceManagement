using Application.Common.Persistence;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;
using static Application.UnitTests.Common.Security.Authorization.TestUtils.RequestWithAuthorizationUtils;

using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UnitTests.TestUtils;

using Moq;
using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Policies;

/// <summary>
/// Unit test for the <see cref="RestrictedUpdatePolicy"/> policy.
/// </summary>
public class RestrictedUpdatePolicyTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly RestrictedUpdatePolicy _policy;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestrictedUpdatePolicyTests"/> class.
    /// </summary>
    public RestrictedUpdatePolicyTests()
    {
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

        _policy = new RestrictedUpdatePolicy(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Verifies an exception is thrown when the request user id is null.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenRequestIdIsNull_ThrowsException()
    {
        var request = new TestRequestWithEmptyUser();

        var currentUser = new IdentityUser("1", [Role.Admin.Name]);

        await FluentActions
            .Invoking(() => _policy.IsAuthorizedAsync(request, currentUser))
            .Should()
            .ThrowAsync<ArgumentException>();
    }

    /// <summary>
    /// Verifies the policy returns true when a user updates themselves.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenUserUpdatesThemselves_ReturnsTrue()
    {
        var userId = UserId.Create("1");
        var request = new TestRequestWithUser(userId.ToString());
        var currentUser = new IdentityUser(userId.ToString(), [Role.Customer.Name]);

        var user = UserUtils.CreateUser(id: userId);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(userId))
            .ReturnsAsync(user);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns true when an admin updates another non-admin user.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenAdminUpdatesNonAdminUser_ReturnsTrue()
    {
        var adminId = UserId.Create("1");
        var nonAdminUserId = UserId.Create("2");

        var request = new TestRequestWithUser(nonAdminUserId.ToString());
        var adminUser = new IdentityUser(adminId.ToString(), [Role.Admin.Name]);

        var nonAdminUser = UserUtils.CreateUser(id: nonAdminUserId, roles: new HashSet<UserRole>()
        {
            UserRole.Create(Role.Customer.Id)
        });

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(nonAdminUserId))
            .ReturnsAsync(nonAdminUser);

        var result = await _policy.IsAuthorizedAsync(request, adminUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns false when an admin tries to update another admin user.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenAdminUpdatesOtherAdmin_ReturnsFalse()
    {
        var adminId = UserId.Create("1");
        var otherAdminId = UserId.Create("2");

        var request = new TestRequestWithUser(otherAdminId.ToString());
        var adminUser = new IdentityUser(adminId.ToString(), [Role.Admin.Name]);

        var otherAdminUser = UserUtils.CreateUser(id: otherAdminId, roles: new HashSet<UserRole>()
        {
            UserRole.Create(Role.Admin.Id)
        });

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(otherAdminId))
            .ReturnsAsync(otherAdminUser);

        var result = await _policy.IsAuthorizedAsync(request, adminUser);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies the policy returns false when the user to be updated does not exist.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenUserToBeUpdatedDoesNotExist_ReturnsFalse()
    {
        var adminId = UserId.Create("1");
        var nonExistingUserId = UserId.Create("9999");

        var request = new TestRequestWithUser(nonExistingUserId.ToString());
        var adminUser = new IdentityUser(adminId.ToString(), [Role.Admin.Name]);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(nonExistingUserId))
            .ReturnsAsync((User?)null);

        var result = await _policy.IsAuthorizedAsync(request, adminUser);

        result.Should().BeFalse();
    }
}
