using Domain.CarrierAggregate;
using Domain.UserAggregate;

namespace Application.Authentication.DTOs.Results;

/// <summary>
/// Represents an authenticated user result.
/// </summary>
public class AuthenticatedUserResult
{
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets the user email address.
    /// </summary>
    public string Email { get; }
    /// <summary>
    /// Gets the user phone.
    /// </summary>
    public string? Phone { get; }

    private AuthenticatedUserResult(User user)
    {
        Id = user.Id.ToString();
        Name = user.Name;
        Email = user.Email.ToString();
        Phone = user.Phone;
    }

    private AuthenticatedUserResult(Carrier carrier)
    {
        Id = carrier.Id.ToString();
        Name = carrier.Name;
        Email = carrier.Email.ToString();
        Phone = carrier.Phone;
    }

    internal static AuthenticatedUserResult FromUser(User user)
    {
        return new AuthenticatedUserResult(user);
    }

    internal static AuthenticatedUserResult FromUser(Carrier carrier)
    {
        return new AuthenticatedUserResult(carrier);
    }
}
