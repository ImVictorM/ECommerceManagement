using Application.Common.DTOs;
using Application.Common.Security.Authorization.Requests;
using SharedKernel.ValueObjects;

namespace Application.Categories.Commands.CreateCategory;

/// <summary>
/// Command to create a new category.
/// </summary>
/// <param name="Name">The category name.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record CreateCategoryCommand(string Name) : IRequestWithAuthorization<CreatedResult>;
