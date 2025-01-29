using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;

using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Command to delete a category.
/// </summary>
/// <param name="Id">The category id.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeleteCategoryCommand(string Id) : IRequestWithAuthorization<Unit>;
