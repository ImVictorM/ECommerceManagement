using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;
using static Application.UnitTests.Common.Security.Authorization.TestUtils.RequestWithAuthorizationUtils;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Policies;

/// <summary>
/// Unit test for the <see cref="SelfOrAdminPolicy"/> policy.
/// </summary>
public class SelfOrAdminPolicyTests
{
    private readonly SelfOrAdminPolicy _policy;

    /// <summary>
    /// Initializes a new instance of the <see cref="SelfOrAdminPolicyTests"/> class.
    /// </summary>
    public SelfOrAdminPolicyTests()
    {
        _policy = new SelfOrAdminPolicy();
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
    /// Verifies the policy returns true when a user accesses themselves.
    /// </summary>
    [Fact]
    public async Task IsAuthorizedAsync_WhenUserAccessesThemselves_ReturnsTrue()
    {
        var userId = "1";
        var request = new TestRequestWithUser(userId);
        var currentUser = new IdentityUser(userId, [Role.Customer.Name]);

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
        var request = new TestRequestWithUser(requestUserId);
        var adminUser = new IdentityUser("1", [Role.Admin.Name]);

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
        var currentUser = new IdentityUser("1", [Role.Customer.Name]);
        var request = new TestRequestWithUser(requestUserId);

        var result = await _policy.IsAuthorizedAsync(request, currentUser);

        result.Should().BeFalse();
    }
}
