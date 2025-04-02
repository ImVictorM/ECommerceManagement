using Application.Common.Validation;
using static Application.UnitTests.Common.Validation.TestUtils.ValidationBehaviorUtils;

using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Common.Validation;

/// <summary>
/// Unit tests for the <see cref="ValidationBehavior{TRequest, TResponse}"/>
/// pipeline behavior.
/// </summary>
public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<TestRequest>> _mockValidator;
    private readonly Mock<RequestHandlerDelegate<TestResponse>> _mockNextDelegate;
    private readonly ValidationBehavior<TestRequest, TestResponse> _behavior;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehaviorTests"/>
    /// class.
    /// </summary>
    public ValidationBehaviorTests()
    {
        _mockValidator = new Mock<IValidator<TestRequest>>();
        _mockNextDelegate = new Mock<RequestHandlerDelegate<TestResponse>>();

        _behavior = new ValidationBehavior<TestRequest, TestResponse>(
            Mock.Of<ILogger<ValidationBehavior<TestRequest, TestResponse>>>(),
            _mockValidator.Object
        );
    }

    /// <summary>
    /// Verifies handling a valid request proceeds to the next delegate.
    /// </summary>
    [Fact]
    public async Task Handle_WithValidRequest_ProceedsToNextDelegate()
    {
        var request = new TestRequest("ValidData");
        var expectedResponse = new TestResponse("Success");

        _mockValidator
            .Setup(v => v.ValidateAsync(
                request,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = []
            });

        _mockNextDelegate
            .Setup(n => n())
            .ReturnsAsync(expectedResponse);

        var response = await _behavior.Handle(
            request,
            _mockNextDelegate.Object,
            default
        );

        response.Should().Be(expectedResponse);
        _mockValidator.Verify(v =>
            v.ValidateAsync(
                request,
                It.IsAny<CancellationToken>()
            ),
           Times.Once()
        );
        _mockNextDelegate.Verify(n => n(), Times.Once());
    }

    /// <summary>
    /// Verifies handling an invalid request throws an validation exception.
    /// </summary>
    [Fact]
    public async Task Handle_WithInvalidRequest_ThrowsException()
    {
        var request = new TestRequest("InvalidData");

        var validationErrors = new List<ValidationFailure>
        {
            new("Data", "Data is invalid.")
        };

        _mockValidator
            .Setup(v => v.ValidateAsync(
                request,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult(validationErrors));

        await FluentActions
            .Invoking(() => _behavior.Handle(
                request,
                _mockNextDelegate.Object,
                default
            ))
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(e => e.Errors.SequenceEqual(validationErrors));

        _mockValidator.Verify(v => v.ValidateAsync(
            request,
            It.IsAny<CancellationToken>()),
            Times.Once()
        );
        _mockNextDelegate.Verify(n => n(), Times.Never());
    }

    /// <summary>
    /// Verifies handling a request without validator proceeds to the next delegate.
    /// </summary>
    [Fact]
    public async Task Handle_WithoutValidator_ProceedsToNextDelegate()
    {
        var request = new TestRequest("NoValidatorData");
        var expectedResponse = new TestResponse("Success");

        var behaviorWithoutValidator =
            new ValidationBehavior<TestRequest, TestResponse>(
                Mock.Of<ILogger<ValidationBehavior<TestRequest, TestResponse>>>()
            );

        _mockNextDelegate
            .Setup(n => n())
            .ReturnsAsync(expectedResponse);

        var response = await behaviorWithoutValidator.Handle(
            request,
            _mockNextDelegate.Object,
            default
        );

        response.Should().Be(expectedResponse);
        _mockNextDelegate.Verify(n => n(), Times.Once());
    }
}
