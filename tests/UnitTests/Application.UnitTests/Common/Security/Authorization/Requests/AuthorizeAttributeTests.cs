using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;

using FluentAssertions;

namespace Application.UnitTests.Common.Security.Authorization.Requests;

/// <summary>
/// Unit tests for the <see cref="AuthorizeAttribute"/> class.
/// </summary>
public class AuthorizeAttributeTests
{
    /// <summary>
    /// Verifies the attribute is created with a valid role.
    /// </summary>
    [Fact]
    public void CreateAttribute_WithValidRoleName_DoesNotThrowException()
    {
        var roleName = Role.Admin.Name;

        FluentActions
            .Invoking(() => new AuthorizeAttribute(roleName: roleName))
            .Should()
            .NotThrow();
    }

    /// <summary>
    /// Verifies an exception is thrown for an invalid role name.
    /// </summary>
    [Fact]
    public void CreateAttribute_WithInvalidRoleName_ThrowsException()
    {
        var invalidRoleName = "InvalidRole";

        FluentActions
            .Invoking(() => new AuthorizeAttribute(roleName: invalidRoleName))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage($"The provided role name is incorrect: {invalidRoleName} (Parameter 'roleName')");
    }

    /// <summary>
    /// Verifies the attribute is created with a valid policy type.
    /// </summary>
    [Fact]
    public void CreateAttribute_WithValidPolicyType_DoesNotThrowException()
    {
        var policyType = typeof(SelfOrAdminPolicy);

        FluentActions
            .Invoking(() => new AuthorizeAttribute(policyType: policyType))
            .Should()
            .NotThrow();
    }

    /// <summary>
    /// Verifies an exception is thrown for an invalid policy type.
    /// </summary>
    [Fact]
    public void CreateAttribute_WithInvalidPolicyType_ThrowsException()
    {
        var invalidPolicyType = typeof(string);

        FluentActions
            .Invoking(() => new AuthorizeAttribute(policyType: invalidPolicyType))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage($"The provided policy type must implement {nameof(IPolicy)} (Parameter 'policyType')");
    }
}
