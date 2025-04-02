using Application.Common.Errors;
using Application.Common.Security.Authorization;
using static Application.UnitTests.Common.Security.Authorization.TestUtils.RequestUtils;

using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Common.Security.Authorization;

/// <summary>
/// Unit tests for the <see cref="AuthorizationBehavior{TRequest, TResponse}"/>
/// pipeline behavior.
/// </summary>
public class AuthorizationBehaviorTests
{
    private readonly AuthorizationBehavior<ITestRequest, TestResponse> _behavior;
    private readonly Mock<IAuthorizationService> _mockAuthService;
    private readonly Mock<RequestHandlerDelegate<TestResponse>> _mockNextDelegate;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationBehaviorTests"/>
    /// class.
    /// </summary>
    public AuthorizationBehaviorTests()
    {
        _mockAuthService = new Mock<IAuthorizationService>();
        _mockNextDelegate = new Mock<RequestHandlerDelegate<TestResponse>>();

        _behavior = new AuthorizationBehavior<ITestRequest, TestResponse>(
            _mockAuthService.Object,
            Mock.Of<ILogger<AuthorizationBehavior<ITestRequest, TestResponse>>>()
        );
    }

    /// <summary>
    /// Verifies handling a request without authorization attributes proceeds
    /// to the next delegate directly.
    /// </summary>
    [Fact]
    public async Task Handle_WithoutAuthorizeAttributes_ProceedToNextDelegate()
    {
        var request = new TestRequestWithoutAuth();
        var expectedResponse = new TestResponse("Success");

        _mockAuthService
            .Setup(s => s.IsCurrentUserAuthorizedAsync(
                It.IsAny<ITestRequest>(),
                It.IsAny<AuthorizationMetadata>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        _mockNextDelegate
            .Setup(n => n())
            .ReturnsAsync(expectedResponse);

        var response = await _behavior.Handle(
            request,
            _mockNextDelegate.Object,
            default
        );

        response.Should().Be(expectedResponse);
    }

    /// <summary>
    /// Verifies handling a request with authorize attributes passes through the
    /// authorization service and proceeds to the next delegate when user is
    /// authorized.
    /// </summary>
    [Fact]
    public async Task Handle_WithAuthorizeAttributesAndAuthorizedUser_ProceedToNextDelegate()
    {
        var requestWithAuth = new TestRequestWithRoleAuthorization();
        var expectedResponse = new TestResponse("Success");

        _mockAuthService
            .Setup(s => s.IsCurrentUserAuthorizedAsync(
                It.IsAny<ITestRequest>(),
                It.IsAny<AuthorizationMetadata>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        _mockNextDelegate
            .Setup(n => n())
            .ReturnsAsync(expectedResponse);

        var response = await _behavior.Handle(
            requestWithAuth,
            _mockNextDelegate.Object,
            default
        );

        response.Should().Be(expectedResponse);
        _mockAuthService.Verify(
            s => s.IsCurrentUserAuthorizedAsync(
                It.IsAny<ITestRequest>(),
                It.IsAny<AuthorizationMetadata>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }

    /// <summary>
    /// Verifies handling a request where the user is not authorized by the
    /// authorization service throws an exception.
    /// </summary>
    [Fact]
    public async Task Handle_WithAuthorizeAttributesAndUserNotAuthorized_ThrowsError()
    {
        var requestWithAuth = new TestRequestWithRoleAuthorization();

        _mockAuthService
            .Setup(s => s.IsCurrentUserAuthorizedAsync(
                It.IsAny<ITestRequest>(),
                It.IsAny<AuthorizationMetadata>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(false);

        await FluentActions
            .Invoking(() => _behavior.Handle(
                requestWithAuth,
                _mockNextDelegate.Object,
                default
            ))
            .Should()
            .ThrowAsync<NotAllowedException>();

        _mockAuthService.Verify(
            s => s.IsCurrentUserAuthorizedAsync(
                It.IsAny<ITestRequest>(),
                It.IsAny<AuthorizationMetadata>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }
}
