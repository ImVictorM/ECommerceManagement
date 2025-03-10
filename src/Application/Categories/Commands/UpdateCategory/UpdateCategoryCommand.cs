using Application.Common.Security.Authorization.Requests;

using MediatR;
using SharedKernel.ValueObjects;

namespace Application.Categories.Commands.UpdateCategory;

/// <summary>
/// Command to update a category.
/// </summary>
/// <param name="Id">The category id.</param>
/// <param name="Name">The new category name.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record UpdateCategoryCommand(string Id, string Name) : IRequestWithAuthorization<Unit>;
