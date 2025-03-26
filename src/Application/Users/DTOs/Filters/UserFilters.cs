namespace Application.Users.DTOs.Filters;

/// <summary>
/// Represents filtering criteria for user queries.
/// </summary>
/// <param name = "IsActive" >
/// Filters users by their activation status.
/// <para>true: Returns only active users.</para>
/// <para>false: Returns only inactive users.</para>
/// <para>null (default): Activation status is not considered.</para>
/// </param>
public record UserFilters(bool? IsActive = null);
