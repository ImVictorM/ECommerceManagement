namespace Application.Common.DTOs.Pagination;

/// <summary>
/// Represents pagination parameters for queries.
/// </summary>
/// <param name="Page">The current page number.</param>
/// <param name="PageSize">The number of items per page.</param>
public record PaginationParams(
    int Page,
    int PageSize
);
