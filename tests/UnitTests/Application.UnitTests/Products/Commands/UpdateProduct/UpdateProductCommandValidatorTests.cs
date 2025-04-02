using Application.Products.Commands.UpdateProduct;
using Application.UnitTests.Products.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Products.Commands.UpdateProduct;

/// <summary>
/// Tests for the <see cref="UpdateProductCommandValidator"/> validator.
/// </summary>
public class UpdateProductCommandValidatorTests
{
    private readonly UpdateProductCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="UpdateProductCommandValidatorTests"/> class.
    /// </summary>
    public UpdateProductCommandValidatorTests()
    {
        _validator = new UpdateProductCommandValidator();
    }

    /// <summary>
    /// Verifies when the product name is empty the name should contain validation
    /// errors.
    /// </summary>
    /// <param name="empty">The empty name.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateUpdateProductCommand_WithEmptyName_ShouldHaveValidationError(
        string empty
    )
    {
        var command = UpdateProductCommandUtils.CreateCommand(name: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    /// <summary>
    /// Verifies when the product description is empty the description should
    /// contain validation errors.
    /// </summary>
    /// <param name="empty">The empty description.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateUpdateProductCommand_WithEmptyDescription_ShouldHaveValidationError(
        string empty
    )
    {
        var command = UpdateProductCommandUtils.CreateCommand(description: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("'Description' must not be empty.");
    }

    /// <summary>
    /// Verifies when the base price of a product is not non-positive the base price
    /// should have validation errors.
    /// </summary>
    /// <param name="invalidPrice">The invalid price.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.NonPositiveNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateUpdateProductCommand_WithNonPositiveBasePrice_ShouldHaveValidationError(
        int invalidPrice
    )
    {
        var command = UpdateProductCommandUtils
            .CreateCommand(basePrice: invalidPrice);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.BasePrice)
            .WithErrorMessage("'Base Price' must be greater than '0'.");
    }

    /// <summary>
    /// Verifies when the categories of a product is empty the categories should
    /// have validation errors.
    /// </summary>
    [Fact]
    public void ValidateUpdateProductCommand_WithEmptyCategoryIds_ShouldHaveValidationError()
    {
        var command = UpdateProductCommandUtils.CreateCommand(categoryIds: []);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.CategoryIds)
            .WithErrorMessage("'Category Ids' must not be empty.");
    }

    /// <summary>
    /// Verifies when the images of a product is empty the images should have
    /// validation errors.
    /// </summary>
    [Fact]
    public void ValidateUpdateProductCommand_WithEmptyImages_ShouldHaveValidationError()
    {
        var command = UpdateProductCommandUtils.CreateCommand(images: []);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Images)
            .WithErrorMessage("'Images' must not be empty.");
    }
}
