using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.DeleteCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.GetCategories;
using Application.Categories.Queries.GetCategoryById;

using Contracts.Categories;

using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Categories;

/// <summary>
/// Defines the category-related endpoints.
/// </summary>
public class CategoryEndpoints : ICarterModule
{
    /// <summary>
    /// The base endpoint for the category resources.
    /// </summary>
    public const string BaseEndpoint = "/products/categories";

    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var categoryGroup = app
            .MapGroup(BaseEndpoint)
            .WithTags("ProductCategories")
            .WithOpenApi();

        categoryGroup
            .MapPost("/", CreateCategory)
            .WithName("CreateCategory")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create New Category",
                Description = "Create a new category. Admin authentication is required."
            })
            .RequireAuthorization();

        categoryGroup
            .MapDelete("/{id:long}", DeleteCategory)
            .WithName("DeleteCategory")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete Category",
                Description = "Deletes an existing category. Admin authentication is required."
            })
            .RequireAuthorization();

        categoryGroup
            .MapPut("/{id:long}", UpdateCategory)
            .WithName("UpdateCategory")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update Category",
                Description = "Updates an existing category. Admin authentication is required."
            })
            .RequireAuthorization();

        categoryGroup
            .MapGet("/", GetCategories)
            .WithName("GetCategories")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Categories",
                Description = "Retrieves all available categories."
            });

        categoryGroup
            .MapGet("/{id:long}", GetCategoryById)
            .WithName("GetCategoryById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Category By Identifier",
                Description = "Retrieves a category by its identifier."
            });
    }

    private async Task<Results<Created, BadRequest, UnauthorizedHttpResult, ForbidHttpResult>> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        var command = mapper.Map<CreateCategoryCommand>(request);

        var result = await sender.Send(command);

        return TypedResults.Created($"{BaseEndpoint}/{result.Id}");
    }

    private async Task<Results<NoContent, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> DeleteCategory(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeleteCategoryCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    private async Task<Results<NoContent, BadRequest, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> UpdateCategory(
        [FromRoute] string id,
        [FromBody] UpdateCategoryRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdateCategoryCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    private async Task<Ok<IEnumerable<CategoryResponse>>> GetCategories(
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetCategoriesQuery();

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<CategoryResponse>));
    }

    private async Task<Results<Ok<CategoryResponse>, NotFound>> GetCategoryById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetCategoryByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<CategoryResponse>(result));
    }
}
