using SharedKernel.ValueObjects;

namespace Application.Common.Security.Authorization;

/// <summary>
/// Represents metadata used for authorizing a request.
/// </summary>
/// <param name="Roles">The required roles for the request.</param>
/// <param name="Policies">The required policies for the request.</param>
public record AuthorizationMetadata(IReadOnlyList<Role> Roles, IReadOnlyList<Type> Policies);

