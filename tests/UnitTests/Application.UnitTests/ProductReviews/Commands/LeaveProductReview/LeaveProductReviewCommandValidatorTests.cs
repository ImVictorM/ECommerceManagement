using Application.ProductReviews.Commands.LeaveProductReview;
using Application.UnitTests.ProductReviews.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.ProductReviews.Commands.LeaveProductReview;

/// <summary>
/// Unit tests for the <see cref="LeaveProductReviewCommandValidator"/>
/// validator.
/// </summary>
public class LeaveProductReviewCommandValidatorTests
{
    private readonly LeaveProductReviewCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="LeaveProductReviewCommandValidatorTests"/> class.
    /// </summary>
    public LeaveProductReviewCommandValidatorTests()
    {
        _validator = new LeaveProductReviewCommandValidator();
    }

    /// <summary>
    /// Verifies when the review content is empty it should contain validation
    /// errors.
    /// </summary>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateLeaveProductReviewCommand_WithEmptyContent_ShouldHaveValidationError(
        string emptyContent
    )
    {
        var command = LeaveProductReviewCommandUtils
            .CreateCommand(content: emptyContent);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();

        result
            .ShouldHaveValidationErrorFor(c => c.Content)
            .WithErrorMessage("'Content' must not be empty.");
    }

    /// <summary>
    /// Verifies when the review title is empty it should contain validation errors.
    /// </summary>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateLeaveProductReviewCommand_WithEmptyTitle_ShouldHaveValidationError(
        string emptyTitle
    )
    {
        var command = LeaveProductReviewCommandUtils
            .CreateCommand(title: emptyTitle);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Title)
            .WithErrorMessage("'Title' must not be empty.");
    }

    /// <summary>
    /// Verifies when the review content exceeds the maximum length it should
    /// contain validation errors.
    /// </summary>
    [Fact]
    public void ValidateLeaveProductReviewCommand_WithExceedingContentLength_ShouldHaveValidationError()
    {
        var longContent = new string('a', 201);
        var command = LeaveProductReviewCommandUtils
            .CreateCommand(content: longContent);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Content)
            .WithErrorMessage(
                "The length of 'Content' must be 200 characters or fewer. " +
                "You entered 201 characters."
            );
    }

    /// <summary>
    /// Verifies when the review title exceeds the maximum length it should contain
    /// validation errors.
    /// </summary>
    [Fact]
    public void ValidateLeaveProductReviewCommand_WithExceedingTitleLength_ShouldHaveValidationError()
    {
        var longTitle = new string('a', 61);
        var command = LeaveProductReviewCommandUtils
            .CreateCommand(title: longTitle);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Title)
            .WithErrorMessage(
                "The length of 'Title' must be 60 characters or fewer. " +
                "You entered 61 characters."
            );
    }
}
