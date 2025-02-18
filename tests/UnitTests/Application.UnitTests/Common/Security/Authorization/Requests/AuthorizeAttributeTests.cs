using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using static Application.UnitTests.Common.Security.Authorization.TestUtils.RequestUtils;

using FluentAssertions;
using SharedKernel.ValueObjects;

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
        var policyType = typeof(SelfOrAdminPolicy<IUserSpecificResource>);

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
            .WithMessage($"The provided policy type must implement {typeof(IPolicy<>).Name} (Parameter 'policyType')");
    }

    /// <summary>
    /// Verifies getting the metadata with a single admin role returns the expected result.
    /// </summary>
    [Fact]
    public void GetAuthorizationMetadata_WithSingleAdminRole_ReturnsCorrectRole()
    {
        var metadata = AuthorizeAttribute.GetAuthorizationMetadata(typeof(TestRequestWithRoleAuthorization));

        metadata.Roles.Should().ContainSingle().Which.Should().Be(Role.Admin);
        metadata.Policies.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies getting the metadata with a single policy returns the expected result.
    /// </summary>
    [Fact]
    public void GetAuthorizationMetadata_WithSinglePolicy_ReturnsCorrectPolicy()
    {
        var metadata = AuthorizeAttribute.GetAuthorizationMetadata(typeof(TestRequestWithPolicyAuthorization));

        metadata.Policies.Should().ContainSingle().Which.Should().Be(typeof(SelfOrAdminPolicy<IUserSpecificResource>));
        metadata.Roles.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies getting the metadata with multiple authorization attributes returns the expected result.
    /// </summary>
    [Fact]
    public void GetAuthorizationMetadata_WithMultipleAuthorization_ReturnsAllRolesAndPolicies()
    {
        var metadata = AuthorizeAttribute.GetAuthorizationMetadata(typeof(TestRequestWithMultipleAuthorization));

        metadata.Roles.Should().ContainSingle().Which.Should().Be(Role.Admin);
        metadata.Policies.Should().ContainSingle().Which.Should().Be(typeof(SelfOrAdminPolicy<IUserSpecificResource>));
    }

    /// <summary>
    /// Verifies getting the metadata without authorization attributes returns an empty metadata.
    /// </summary>
    [Fact]
    public void GetAuthorizationMetadata_WithNoAuthorization_ReturnsEmptyMetadata()
    {
        var metadata = AuthorizeAttribute.GetAuthorizationMetadata(typeof(TestRequestWithoutAuth));

        metadata.Roles.Should().BeEmpty();
        metadata.Policies.Should().BeEmpty();
    }
}
