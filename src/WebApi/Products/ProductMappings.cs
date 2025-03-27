using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Commands.UpdateProductInventory;
using Application.Products.DTOs.Results;

using Contracts.Products;

using Mapster;

namespace WebApi.Products;

internal sealed class ProductMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductRequest, CreateProductCommand>();

        config
            .NewConfig<(string Id, UpdateProductRequest Request), UpdateProductCommand>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<
                (string Id, UpdateProductInventoryRequest Request),
                UpdateProductInventoryCommand
            >()
            .Map(dest => dest.ProductId, src => src.Id)
            .Map(dest => dest.QuantityToIncrement, src => src.Request.QuantityToIncrement);

        config.NewConfig<ProductResult, ProductResponse>();
    }
}
