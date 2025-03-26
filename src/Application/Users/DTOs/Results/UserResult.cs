using Domain.UserAggregate;

using SharedKernel.ValueObjects;

namespace Application.Users.DTOs.Results;

/// <summary>
/// Represents a user result.
/// </summary>
public class UserResult
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
    /// <summary>
    /// Gets the user addresses.
    /// </summary>
    public IReadOnlyList<Address> Addresses { get; }
    /// <summary>
    /// Gets the user role names.
    /// </summary>
    public IReadOnlyList<string> Roles { get; }

    private UserResult(User user)
    {
        Id = user.Id.ToString();
        Name = user.Name;
        Email = user.Email.ToString();
        Phone = user.Phone;
        Addresses = user.UserAddresses.ToList();
        Roles = user.UserRoles.Select(r => r.Role.Name).ToList();
    }

    internal static UserResult FromUser(User user)
    {
        return new UserResult(user);
    }
};
