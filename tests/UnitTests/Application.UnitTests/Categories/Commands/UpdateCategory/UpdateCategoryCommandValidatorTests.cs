using Application.Categories.Commands.UpdateCategory;
using Application.UnitTests.Categories.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentValidation.TestHelper;

namespace Application.UnitTests.Categories.Commands.UpdateCategory;

/// <summary>
/// Unit tests for the <see cref="UpdateCategoryCommandValidator"/> validator.
/// </summary>
public class UpdateCategoryCommandValidatorTests
{
    private readonly UpdateCategoryCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="UpdateCategoryCommandValidatorTests"/> class.
    /// </summary>
    public UpdateCategoryCommandValidatorTests()
    {
        _validator = new UpdateCategoryCommandValidator();
    }

    /// <summary>
    /// Verifies when the category name is invalid the validator should have an error.
    /// </summary>
    /// <param name="invalidName">The category name.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateUpdateCategoryCommand_WithInvalidName_ShouldHaveValidationError(
        string invalidName
    )
    {
        var command = UpdateCategoryCommandUtils.CreateCommand(name: invalidName);

        var result = _validator.TestValidate(command);

        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }
}
