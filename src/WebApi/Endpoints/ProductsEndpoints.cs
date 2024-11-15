using Application.Products.Commands.CreateProduct;
using Application.Products.Queries.GetProductById;
using Carter;
using Contracts.Products;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization.AdminRequired;

namespace WebApi.Endpoints;

/// <summary>
/// Defines product-related routes.
/// </summary>
public class ProductsEndpoints : ICarterModule
{
    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var productGroup = app
            .MapGroup("/products")
            .WithTags("Products")
            .WithOpenApi();

        productGroup
            .MapPost("/", CreateProduct)
            .WithName("CreateProduct")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create Product",
                Description = "Creates a new product. Administrators only."
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        productGroup
            .MapGet("/{id:long}", GetProductById)
            .WithName("GetProductById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Product By Identifier",
                Description = "Retrieves a product by identifier value. Does not require authentication."
            });
    }

    private async Task<Results<Ok<ProductResponse>, BadRequest>> GetProductById(
        [FromRoute] string id,
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetProductByIdQuery(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<ProductResponse>(result));
    }

    private async Task<Results<Created, BadRequest, UnauthorizedHttpResult>> CreateProduct(
        [FromBody] CreateProductRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        var command = mapper.Map<CreateProductCommand>(request);

        var createdResponse = await sender.Send(command);

        return TypedResults.Created($"/products/{createdResponse.Id}");
    }
}
