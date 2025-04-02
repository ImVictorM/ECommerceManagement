using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Represents a command to delete a category.
/// </summary>
/// <param name="Id">The category identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeleteCategoryCommand(string Id) : IRequestWithAuthorization<Unit>;
