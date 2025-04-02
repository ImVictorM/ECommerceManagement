using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.DeleteCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.GetCategories;
using Application.Categories.Queries.GetCategoryById;

using Contracts.Categories;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using MediatR;
using Carter;

namespace WebApi.Categories;

/// <summary>
/// Provides endpoints for the category features.
/// </summary>
public sealed class CategoryEndpoints : ICarterModule
{
    private const string BaseEndpoint = "/products/categories";

    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var categoryGroup = app
            .MapGroup(BaseEndpoint)
            .WithTags("ProductCategories")
            .WithOpenApi();

        categoryGroup
            .MapPost("/", CreateCategory)
            .WithName(nameof(CreateCategory))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create New Category",
                Description =
                "Create a new category. " +
                "Admin authentication is required."
            })
            .RequireAuthorization();

        categoryGroup
            .MapDelete("/{id:long}", DeleteCategory)
            .WithName(nameof(DeleteCategory))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete Category",
                Description =
                "Deletes an existent category. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The category identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();

        categoryGroup
            .MapPut("/{id:long}", UpdateCategory)
            .WithName(nameof(UpdateCategory))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update Category",
                Description =
                "Updates an existent category. " +
                "Admin authentication is required.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The category identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            })
            .RequireAuthorization();

        categoryGroup
            .MapGet("/", GetCategories)
            .WithName(nameof(GetCategories))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Categories",
                Description = "Retrieves all available categories."
            });

        categoryGroup
            .MapGet("/{id:long}", GetCategoryById)
            .WithName(nameof(GetCategoryById))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Category By Id",
                Description = "Retrieves a category by its identifier.",
                Parameters =
                [
                    new()
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Description = "The category identifier.",
                        Required = true,
                        Schema = new() { Type = "integer", Format = "int64" }
                    }
                ],
            });
    }

    internal async Task<Results<
        Created,
        BadRequest,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        var command = mapper.Map<CreateCategoryCommand>(request);

        var result = await sender.Send(command);

        return TypedResults.Created($"{BaseEndpoint}/{result.Id}");
    }

    internal async Task<Results<
        NoContent,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> DeleteCategory(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeleteCategoryCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    internal async Task<Results<
        NoContent,
        BadRequest,
        NotFound,
        UnauthorizedHttpResult,
        ForbidHttpResult
    >> UpdateCategory(
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

    internal async Task<Ok<List<CategoryResponse>>> GetCategories(
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetCategoriesQuery();

        var result = await sender.Send(query);

        var response = result
            .Select(mapper.Map<CategoryResponse>)
            .ToList();

        return TypedResults.Ok(response);
    }

    internal async Task<Results<Ok<CategoryResponse>, NotFound>> GetCategoryById(
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
