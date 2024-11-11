using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization.DeactivateUser;

/// <summary>
/// Defines a requirement to deactivate an user.
/// </summary>
public class DeactivateUserRequirement : IAuthorizationRequirement;
