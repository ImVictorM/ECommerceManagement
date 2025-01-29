using Application.Common.Persistence;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;
using Application.Common.Security.Authorization.Requests;
using static Application.UnitTests.Common.Security.Authorization.TestUtils.RequestWithAuthorizationUtils;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Common.Security.Authorization.Policies;

/// <summary>
/// Unit test for the <see cref="RestrictedDeactivationPolicy{T}"/> policy.
/// </summary>
public class RestrictedDeactivationPolicyTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly RestrictedDeactivationPolicy<IUserSpecificResource> _policy;

    /// <summary>
    /// Initiates a new instance of the <see cref="RestrictedDeactivationPolicyTests"/> class.
    /// </summary>
    public RestrictedDeactivationPolicyTests()
    {
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

        _policy = new RestrictedDeactivationPolicy<IUserSpecificResource>(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Verifies the authorize method returns true when a non admin user tries to deactivate themselves.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenNonAdminCurrentUserDeactivatesThemselves_ReturnsTrue()
    {
        var userId = "1";
        var request = new TestRequestWithUser(userId);
        var currentCustomerUser = new IdentityUser(userId, [Role.Customer.Name]);

        var result = await _policy.IsAuthorizedAsync(request, currentCustomerUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the authorize method returns false when an admin tries to deactivate themselves.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_CurrentAdminDeactivatesThemselves_ReturnsFalse()
    {
        var userId = "1";
        var request = new TestRequestWithUser(userId);
        var currentAdminUser = new IdentityUser(userId, [Role.Admin.Name]);

        var result = await _policy.IsAuthorizedAsync(request, currentAdminUser);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies the authorize methods returns true when an admin tries to deactivate another
    /// non administrator user.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_AdminDeactivatesNonAdminUser_ReturnsTrue()
    {
        var currentAdminUserId = UserId.Create("1");
        var userToBeDeactivatedId = UserId.Create("2");

        var request = new TestRequestWithUser(userToBeDeactivatedId.ToString());

        var currentUser = new IdentityUser(currentAdminUserId.ToString(), [Role.Admin.Name]);

        var userToBeDeactivated = UserUtils.CreateUser(id: userToBeDeactivatedId, roles: new HashSet<UserRole>()
        {
            UserRole.Create(Role.Customer.Id)
        });

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(userToBeDeactivatedId))
            .ReturnsAsync(userToBeDeactivated);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the authorize method returns false when an administrator tries to deactivate
    /// another administrator.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_AdminDeactivatesAdminUser_ReturnsFalse()
    {
        var currentAdminUser = UserId.Create("1");
        var otherAdminToBeDeactivatedId = UserId.Create("2");

        var request = new TestRequestWithUser(otherAdminToBeDeactivatedId.ToString());

        var currentUser = new IdentityUser(currentAdminUser.ToString(), [Role.Admin.Name]);

        var otherAdminToBeDeactivated = UserUtils.CreateUser(id: otherAdminToBeDeactivatedId, roles: new HashSet<UserRole>()
        {
            UserRole.Create(Role.Admin.Id)
        });

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(otherAdminToBeDeactivatedId))
            .ReturnsAsync(otherAdminToBeDeactivated);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies the authorize method returns false when the user to be deactivated
    /// does not exist.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_UserToBeDeactivatedDoesNotExist_ReturnsFalse()
    {
        var currentAdminUserId = UserId.Create("1");
        var nonExistingUserId = UserId.Create("9999");

        var request = new TestRequestWithUser(nonExistingUserId.ToString());

        var currentUser = new IdentityUser(currentAdminUserId.ToString(), [Role.Admin.Name]);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(nonExistingUserId))
            .ReturnsAsync((User?)null);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeFalse();
    }
}
