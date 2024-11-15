using Application.Products.Commands.CreateProduct;
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
        config.NewConfig<ProductResult, ProductResponse>()
            .Map(dest => dest.OriginalPrice, src => src.Product.BasePrice)
            .Map(dest => dest.PriceWithDiscount, src => src.Product.GetPriceAfterDiscounts())
            .Map(dest => dest.QuantityAvailable, src => src.Product.Inventory.QuantityAvailable)
            .Map(dest => dest.Images, src => src.Product.ProductImages.Select(pi => pi.Url))
            .Map(dest => dest.Categories, src => src.Product.GetCategoryNames())
            .Map(dest => dest.Id, src => src.Product.Id.Value)
            .Map(dest => dest, src => src.Product);
    }
}
