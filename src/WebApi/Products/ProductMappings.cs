using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Commands.UpdateProductInventory;
using Application.Products.DTOs;

using Contracts.Products;

using Mapster;

namespace WebApi.Products;

internal sealed class ProductMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductRequest, CreateProductCommand>()
            .ConstructUsing(src => new CreateProductCommand(
                src.Name,
                src.Description,
                src.InitialQuantity,
                src.BasePrice,
                src.CategoryIds,
                src.Images
            ));

        config.NewConfig<(string Id, UpdateProductRequest Request), UpdateProductCommand>()
            .ConstructUsing(src => new UpdateProductCommand(
                src.Id,
                src.Request.Name,
                src.Request.Description,
                src.Request.BasePrice,
                src.Request.Images,
                src.Request.CategoryIds
            ));

        config.NewConfig<(string Id, UpdateProductInventoryRequest Request), UpdateProductInventoryCommand>()
            .ConstructUsing(src => new UpdateProductInventoryCommand(
                src.Id,
                src.Request.QuantityToIncrement
            ));

        config.NewConfig<ProductResult, ProductResponse>()
            .Map(dest => dest.Id, src => src.Product.Id.ToString())
            .Map(dest => dest.Name, src => src.Product.Name)
            .Map(dest => dest.Description, src => src.Product.Description)
            .Map(dest => dest.BasePrice, src => src.Product.BasePrice)
            .Map(dest => dest.PriceWithDiscount, src => src.PriceWithDiscount)
            .Map(dest => dest.QuantityAvailable, src => src.Product.Inventory.QuantityAvailable)
            .Map(dest => dest.Categories, src => src.Categories)
            .Map(dest => dest.Images, src => src.Product.ProductImages.Select(pi => pi.Url));
    }
}
