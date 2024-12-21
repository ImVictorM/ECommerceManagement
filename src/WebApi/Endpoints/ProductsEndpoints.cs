using Carter;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization.AdminRequired;

using Contracts.Products;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeactivateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Commands.UpdateProductInventory;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProducts;

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
                Description = "Creates a new product. Admin authentication is required."
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        productGroup
            .MapGet("/{id:long}", GetProductById)
            .WithName("GetProductById")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Product By Identifier",
                Description = "Retrieves an active product by its identifier. No authentication is required."
            });

        productGroup
            .MapGet("/", GetAllProducts)
            .WithName("GetAllProducts")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Products",
                Description = @"Retrieves all active products.
It will retrieve the first 20 products if no limit is specified.
It is possible to filter the products by the specified categories appending &category={category_name} to the URL.
No authentication is required."
            });

        productGroup
            .MapPut("/{id:long}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update Product",
                Description = "Updates the details of an active product. Admin authentication is required."
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        productGroup
            .MapDelete("/{id:long}", DeactivateProduct)
            .WithName("DeactivateProduct")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Deactivate Product",
                Description = "Deactivates a product and set the inventory to 0 items. Admin authentication is required."
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        productGroup
            .MapPut("/{id:long}/inventory", UpdateProductInventory)
            .WithName("UpdateProductInventory")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update Product Inventory",
                Description = "Increments the inventory quantity available for an active product. Admin authentication is required."
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

    }

    private async Task<Results<Ok<ProductResponse>, NotFound>> GetProductById(
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

    private async Task<Ok<IEnumerable<ProductResponse>>> GetAllProducts(
        ISender sender,
        IMapper mapper,
        [FromQuery(Name = "limit")] int? limit = null,
        [FromQuery(Name = "category")] string[]? categories = null
    )
    {
        var query = new GetProductsQuery(limit, categories);

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Select(mapper.Map<ProductResponse>));
    }

    private async Task<Results<NoContent, BadRequest, NotFound, UnauthorizedHttpResult>> UpdateProduct(
        [FromRoute] string id,
        [FromBody] UpdateProductRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        var command = mapper.Map<UpdateProductCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    private async Task<Results<NoContent, NotFound, UnauthorizedHttpResult>> DeactivateProduct(
        [FromRoute] string id,
        ISender sender
    )
    {
        var command = new DeactivateProductCommand(id);

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    private async Task<Results<NoContent, BadRequest, NotFound, UnauthorizedHttpResult>> UpdateProductInventory
    (
        [FromRoute] string id,
        [FromBody] UpdateProductInventoryRequest request,
        IMapper mapper,
        ISender sender
    )
    {
        var command = mapper.Map<UpdateProductInventoryCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
