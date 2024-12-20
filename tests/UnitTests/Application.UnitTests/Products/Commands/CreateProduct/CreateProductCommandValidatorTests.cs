using Application.Products.Commands.CreateProduct;
using Application.UnitTests.Products.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Products.Commands.CreateProduct;

/// <summary>
/// Tests fot the <see cref="CreateProductCommandValidator"/> validator.
/// </summary>
public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductCommandValidatorTests"/> class.
    /// </summary>
    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    /// <summary>
    /// Tests when the product name is empty the name should contain validation errors.
    /// </summary>
    /// <param name="empty">The empty name.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateCreateProductCommand_WhenNameIsEmpty_ShouldHaveValidationErrors(string empty)
    {
        var command = CreateProductCommandUtils.CreateCommand(name: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage("'Name' must not be empty.");
    }

    /// <summary>
    /// Tests when the product description is empty the description should contain validation errors.
    /// </summary>
    /// <param name="empty">The empty description.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateCreateProductCommand_WhenDescriptionIsEmpty_ShouldHaveValidationErrors(string empty)
    {
        var command = CreateProductCommandUtils.CreateCommand(description: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Description).WithErrorMessage("'Description' must not be empty.");
    }

    /// <summary>
    /// Tests when the base price of a product is not greater than zero the base price should have validation errors.
    /// </summary>
    /// <param name="invalidPrice">The invalid price.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.NonPositiveNumbers), MemberType = typeof(ValidationTestData))]
    public void ValidateCreateProductCommand_WhenBasePriceIsNotGreaterThanZero_ShouldHaveValidationErrors(int invalidPrice)
    {
        var command = CreateProductCommandUtils.CreateCommand(basePrice: invalidPrice);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.BasePrice).WithErrorMessage("'Base Price' must be greater than '0'.");
    }

    /// <summary>
    /// Tests when the initial quantity of a product is not greater than zero the initial quantity should have validation errors.
    /// </summary>
    /// <param name="invalidInitialQuantity">The invalid initial quantity.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.NonPositiveNumbers), MemberType = typeof(ValidationTestData))]
    public void ValidateCreateProductCommand_WhenInitialQuantityIsNotGreaterThanZero_ShouldHaveValidationErrors(int invalidInitialQuantity)
    {
        var command = CreateProductCommandUtils.CreateCommand(initialQuantity: invalidInitialQuantity);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.InitialQuantity).WithErrorMessage("'Initial Quantity' must be greater than '0'.");
    }

    /// <summary>
    /// Tests when the categories of a product is empty the categories should have validation errors.
    /// </summary>
    [Fact]
    public void ValidateCreateProductCommand_WhenCategoriesAreEmpty_ShouldHaveValidationErrors()
    {
        var command = CreateProductCommandUtils.CreateCommand(categories: []);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.CategoryIds).WithErrorMessage("'Categories' must not be empty.");
    }

    /// <summary>
    /// Tests when the images of a product is empty the images should have validation errors.
    /// </summary>
    [Fact]
    public void ValidateCreateProductCommand_WhenImagesAreEmpty_ShouldHaveValidationErrors()
    {
        var command = CreateProductCommandUtils.CreateCommand(images: []);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Images).WithErrorMessage("'Images' must not be empty.");
    }
}
