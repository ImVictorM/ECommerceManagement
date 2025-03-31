using Application.ProductReviews.Commands.LeaveProductReview;
using Application.UnitTests.ProductReviews.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.ProductReviews.Commands.LeaveProductReview;

/// <summary>
/// Unit tests for the <see cref="LeaveProductReviewCommandValidator"/> validator.
/// </summary>
public class LeaveProductFeedbackCommandValidatorTests
{
    private readonly LeaveProductReviewCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="LeaveProductFeedbackCommandValidatorTests"/> class.
    /// </summary>
    public LeaveProductFeedbackCommandValidatorTests()
    {
        _validator = new LeaveProductReviewCommandValidator();
    }

    /// <summary>
    /// Tests when the feedback content is empty it should contain validation errors.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateLeaveProductFeedbackCommand_WhenContentIsEmpty_ShouldHaveValidationErrors(
        string emptyContent
    )
    {
        var command = LeaveProductReviewCommandUtils.CreateCommand(content: emptyContent);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();

        result
            .ShouldHaveValidationErrorFor(c => c.Content)
            .WithErrorMessage("'Content' must not be empty.");
    }

    /// <summary>
    /// Tests when the feedback title is empty it should contain validation errors.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateLeaveProductFeedbackCommand_WhenTitleIsEmpty_ShouldHaveValidationErrors(
        string emptyTitle
    )
    {
        var command = LeaveProductReviewCommandUtils.CreateCommand(title: emptyTitle);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Title)
            .WithErrorMessage("'Title' must not be empty.");
    }

    /// <summary>
    /// Tests when the feedback content exceeds the maximum length it should contain validation errors.
    /// </summary>
    [Fact]
    public void ValidateLeaveProductFeedbackCommand_WhenContentExceedsMaxLength_ShouldHaveValidationErrors()
    {
        var longContent = new string('a', 201);
        var command = LeaveProductReviewCommandUtils.CreateCommand(content: longContent);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Content)
            .WithErrorMessage("The length of 'Content' must be 200 characters or fewer. You entered 201 characters.");
    }

    /// <summary>
    /// Tests when the feedback title exceeds the maximum length it should contain validation errors.
    /// </summary>
    [Fact]
    public void ValidateLeaveProductFeedbackCommand_WhenTitleExceedsMaxLength_ShouldHaveValidationErrors()
    {
        var longTitle = new string('a', 61);
        var command = LeaveProductReviewCommandUtils.CreateCommand(title: longTitle);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Title)
            .WithErrorMessage("The length of 'Title' must be 60 characters or fewer. You entered 61 characters.");
    }
}
