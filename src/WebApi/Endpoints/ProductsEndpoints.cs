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
using Application.Products.Queries.GetProductCategories;
using Application.Products.Queries.GetProducts;
using Application.Products.Commands.UpdateProductDiscounts;

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

        productGroup
            .MapGet("/", GetAllProducts)
            .WithName("GetAllProducts")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Products",
                Description = "Retrieves all products. Will retrieve the first 20 products if no limit is specified. Can filter the products by the specified categories. Does not require authentication."
            });

        productGroup
            .MapGet("/categories", GetAllProductCategories)
            .WithName("GetAllProductCategories")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get Product Categories",
                Description = "Retrieves all available product categories. Does not require authentication"
            });

        productGroup
            .MapPut("/{id:long}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update Product",
                Description = "Updates the base details of a product. Needs authentication as admin"
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        productGroup
            .MapDelete("/{id:long}", DeactivateProduct)
            .WithName("DeactivateProduct")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Deactivate Product",
                Description = "Deactivates a product and set the inventory to 0 items. Needs authentication as admin"
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        productGroup
            .MapPut("/{id:long}/inventory", UpdateProductInventory)
            .WithName("UpdateProductInventory")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update Product Inventory",
                Description = "Increments a product's inventory quantity available. Needs authentication as admin"
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

        productGroup
            .MapPut("/{id:long}/discounts", UpdateProductDiscounts)
            .WithName("UpdateProductDiscounts")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update Product Discounts",
                Description = "Updates the list of discounts related to a product. Needs authentication as admin"
            })
            .RequireAuthorization(AdminRequiredPolicy.Name);

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

    private async Task<Ok<ProductListResponse>> GetAllProducts(
        ISender sender,
        IMapper mapper,
        [FromQuery(Name = "limit")] int? limit = null,
        [FromQuery(Name = "category")] string[]? categories = null
    )
    {
        var query = new GetProductsQuery(limit, categories);

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<ProductListResponse>(result));
    }

    private async Task<Ok<ProductCategoriesResponse>> GetAllProductCategories(
        ISender sender,
        IMapper mapper
    )
    {
        var query = new GetProductCategoriesQuery();

        var result = await sender.Send(query);

        return TypedResults.Ok(mapper.Map<ProductCategoriesResponse>(result));
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

    private async Task<Results<NoContent, BadRequest, NotFound, UnauthorizedHttpResult>> UpdateProductDiscounts(
        [FromRoute] string id,
        [FromBody] UpdateProductDiscountsRequest request,
        ISender sender,
        IMapper mapper
    )
    {
        var command = mapper.Map<UpdateProductDiscountsCommand>((id, request));

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
