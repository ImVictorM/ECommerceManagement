using Application.Common.Security.Authorization.Requests;

using MediatR;
using SharedKernel.ValueObjects;

namespace Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Command to delete a category.
/// </summary>
/// <param name="Id">The category id.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeleteCategoryCommand(string Id) : IRequestWithAuthorization<Unit>;
