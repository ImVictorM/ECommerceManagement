using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using SharedKernel.ValueObjects;

namespace Application.UnitTests.Common.Security.Authorization.TestUtils;

/// <summary>
/// Utilities that defines requests with authorization for testing.
/// </summary>
public static class RequestUtils
{
    /// <summary>
    /// Represents a test request that implements <see cref="IRequestWithAuthorization{T}"/>.
    /// </summary>
    public interface ITestRequest : IRequestWithAuthorization<TestResponse>;

    /// <summary>
    /// Represents a response.
    /// </summary>
    /// <param name="Message">The response message.</param>
    public record TestResponse(string Message);

    /// <summary>
    /// Represents a request without authorization attributes.
    /// </summary>
    public class TestRequestWithoutAuth : ITestRequest;

    /// <summary>
    /// Represents a user related request without authorization attributes.
    /// </summary>
    /// <param name="UserId">The user id.</param>
    public record TestRequestWithoutAuthUserRelated(string UserId) : ITestRequest, IUserSpecificResource;

    /// <summary>
    /// Represents a shipment related request without authorization attributes.
    /// </summary>
    /// <param name="ShipmentId">The shipment id.</param>
    public record TestRequestWithoutAuthShipmentRelated(string ShipmentId) : ITestRequest, IShipmentSpecificResource;

    /// <summary>
    /// Represents a request with a role authorization attribute.
    /// </summary>
    [Authorize(roleName: nameof(Role.Admin))]
    public class TestRequestWithRoleAuthorization : ITestRequest;

    /// <summary>
    /// Represents a request with a policy authorization attribute.
    /// </summary>
    [Authorize(policyType: typeof(SelfOrAdminPolicy<IUserSpecificResource>))]
    public class TestRequestWithPolicyAuthorization : ITestRequest;

    /// <summary>
    /// Represents a request with policy and role authorization attributes.
    /// </summary>
    [Authorize(roleName: nameof(Role.Admin))]
    [Authorize(policyType: typeof(SelfOrAdminPolicy<IUserSpecificResource>))]
    public class TestRequestWithMultipleAuthorization : ITestRequest;

}
