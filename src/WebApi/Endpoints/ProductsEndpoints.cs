using Application.Products.Commands.CreateProduct;
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
