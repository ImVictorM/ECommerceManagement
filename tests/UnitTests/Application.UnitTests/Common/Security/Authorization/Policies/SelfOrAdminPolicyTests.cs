using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Identity;
using Application.Common.Security.Authorization.Requests;
using static Application.UnitTests.Common.Security.Authorization.TestUtils.RequestUtils;

using SharedKernel.ValueObjects;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Policies;

/// <summary>
/// Unit test for the <see cref="SelfOrAdminPolicy{T}"/> policy.
/// </summary>
public class SelfOrAdminPolicyTests
{
    private readonly SelfOrAdminPolicy<IUserSpecificResource> _policy;

    /// <summary>
    /// Initializes a new instance of the <see cref="SelfOrAdminPolicyTests"/> class.
    /// </summary>
    public SelfOrAdminPolicyTests()
    {
        _policy = new SelfOrAdminPolicy<IUserSpecificResource>();
    }

    /// <summary>
    /// Verifies the policy returns true when a user accesses themselves.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenUserAccessesThemselves_ReturnsTrue()
    {
        var userId = "1";
        var request = new TestRequestWithoutAuthUserRelated(userId);
        var currentUser = new IdentityUser(userId, [Role.Customer]);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns true when an admin accesses another user.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenAdminAccessesAnotherUser_ReturnsTrue()
    {
        var requestUserId = "2";
        var request = new TestRequestWithoutAuthUserRelated(requestUserId);
        var adminUser = new IdentityUser("1", [Role.Admin]);

        var result = await _policy.IsAuthorizedAsync(request, adminUser);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the policy returns false when a user tries to access another user.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenUserAccessesAnotherUser_ReturnsFalse()
    {
        var requestUserId = "2";
        var currentUser = new IdentityUser("1", [Role.Customer]);
        var request = new TestRequestWithoutAuthUserRelated(requestUserId);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeFalse();
    }
}
