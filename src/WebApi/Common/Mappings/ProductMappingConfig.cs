using Application.Products.Commands.CreateProduct;
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
    }
}
