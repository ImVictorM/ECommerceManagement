using Application.Categories.Commands.CreateCategory;
using Application.UnitTests.Categories.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentValidation.TestHelper;

namespace Application.UnitTests.Categories.Commands.CreateCategory;

/// <summary>
/// Unit tests for the <see cref="CreateCategoryCommandValidator"/> validator.
/// </summary>
public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCategoryCommandValidatorTests"/> class.
    /// </summary>
    public CreateCategoryCommandValidatorTests()
    {
        _validator = new CreateCategoryCommandValidator();
    }

    /// <summary>
    /// Tests when the category name is invalid the validator should have an error.
    /// </summary>
    /// <param name="invalidName">The category name.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateCreateCategoryCommand_WithInvalidName_ShouldHaveValidationError(string invalidName)
    {
        var command = CreateCategoryCommandUtils.CreateCommand(name: invalidName);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage("'Name' must not be empty.");
    }
}
