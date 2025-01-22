using Application.Common.Extensions.Validations;

using FluentValidation;

namespace Application.UnitTests.Common.Extensions.Validations.TestUtils;

/// <summary>
/// Utilities to test the rule builder extensions.
/// </summary>
public static class RuleBuilderUtils
{
    /// <summary>
    /// Represents an email validator;
    /// </summary>
    public class TestEmailValidator : AbstractValidator<TestEmailInput>
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="TestEmailValidator"/> class.
        /// </summary>
        public TestEmailValidator()
        {
            RuleFor(x => x.Email).IsValidEmail();
        }
    }

    /// <summary>
    /// Represents a user name validator.
    /// </summary>
    public class TestNameValidator : AbstractValidator<TestNameInput>
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="TestNameValidator"/> class.
        /// </summary>
        public TestNameValidator()
        {
            RuleFor(x => x.Name).IsValidUserName();
        }
    }

    /// <summary>
    /// Represents an input for the email validator.
    /// </summary>
    public class TestEmailInput
    {
        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents an input for the user name validator.
    /// </summary>
    public class TestNameInput
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
