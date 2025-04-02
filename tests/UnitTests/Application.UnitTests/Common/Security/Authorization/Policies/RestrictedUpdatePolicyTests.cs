using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Identity;
using Application.Common.Security.Authorization.Requests;
using static Application.UnitTests.Common.Security.Authorization.TestUtils.RequestUtils;

using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UnitTests.TestUtils;

using SharedKernel.ValueObjects;

using Moq;
using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Policies;

/// <summary>
/// Unit test for the <see cref="RestrictedUpdatePolicy{T}"/> policy.
/// </summary>
public class RestrictedUpdatePolicyTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly RestrictedUpdatePolicy<IUserSpecificResource> _policy;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="RestrictedUpdatePolicyTests"/> class.
    /// </summary>
    public RestrictedUpdatePolicyTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        _policy = new RestrictedUpdatePolicy<IUserSpecificResource>(
            _mockUserRepository.Object
        );
    }

    /// <summary>
    /// Verifies the policy returns true when a user updates themselves.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenUserUpdatesThemselves_ReturnsTrue()
    {
        var userId = UserId.Create("1");
        var request = new TestRequestWithoutAuthUserRelated(userId.ToString());
        var currentUser = new IdentityUser(userId.ToString(), [Role.Customer]);

        var user = UserUtils.CreateCustomer(id: userId);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                userId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(user);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns true when an admin updates another non-admin
    /// user.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenAdminUpdatesNonAdminUser_ReturnsTrue()
    {
        var adminId = UserId.Create("1");
        var nonAdminUserId = UserId.Create("2");

        var request = new TestRequestWithoutAuthUserRelated(
            nonAdminUserId.ToString()
        );

        var adminUser = new IdentityUser(adminId.ToString(), [Role.Admin]);

        var nonAdminUser = UserUtils.CreateCustomer(id: nonAdminUserId);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                nonAdminUserId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(nonAdminUser);

        var result = await _policy.IsAuthorizedAsync(request, adminUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns false when an admin tries to update another
    /// admin user.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenAdminUpdatesOtherAdmin_ReturnsFalse()
    {
        var adminId = UserId.Create("1");
        var otherAdminId = UserId.Create("2");

        var request = new TestRequestWithoutAuthUserRelated(
            otherAdminId.ToString()
        );
        var adminUser = new IdentityUser(adminId.ToString(), [Role.Admin]);

        var otherAdminUser = UserUtils.CreateAdmin(id: otherAdminId);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                otherAdminId,
                It.IsAny<CancellationToken>()
            ))
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
        var nonExistentUserId = UserId.Create("9999");

        var request = new TestRequestWithoutAuthUserRelated(
            nonExistentUserId.ToString()
        );

        var adminUser = new IdentityUser(adminId.ToString(), [Role.Admin]);

        _mockUserRepository
            .Setup(r => r.FindByIdAsync(
                nonExistentUserId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((User?)null);

        var result = await _policy.IsAuthorizedAsync(request, adminUser);

        result.Should().BeFalse();
    }
}
