using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Commands.UpdateProductInventory;
using Application.Products.Queries.Common.DTOs;
using Contracts.Products;
using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configure mapping between product objects.
/// </summary>
public class ProductMappingConfig : IRegister
{
    /// <summary>
    /// Register mapping configurations related to products.
    /// </summary>
    /// <param name="config">The global configuration object.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductRequest, CreateProductCommand>();

        config.NewConfig<(string Id, UpdateProductRequest Request), UpdateProductCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<(string Id, UpdateProductInventoryRequest Request), UpdateProductInventoryCommand>()
            .Map(dest => dest.ProductId, src => src.Id)
            .Map(dest => dest, src => src.Request);

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
