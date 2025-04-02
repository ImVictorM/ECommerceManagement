using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

/// <summary>
/// Represents a command to update a category.
/// </summary>
/// <param name="Id">The category identifier.</param>
/// <param name="Name">The new category name.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record UpdateCategoryCommand(string Id, string Name)
    : IRequestWithAuthorization<Unit>;
