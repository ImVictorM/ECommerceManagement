namespace Application.Common.Security.Authorization.Requests;

/// <summary>
///  Represents a request that is specific to a user resource.
/// </summary>
public interface IUserSpecificResource
{
    /// <summary>
    /// Gets the user id for user specific requests.
    /// </summary>
    public string UserId { get; }
}
