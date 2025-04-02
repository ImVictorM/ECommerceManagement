using Application.Sales.Commands.CreateSale;
using Application.Sales.Commands.UpdateSale;
using Application.Sales.DTOs.Results;

using Contracts.Sales;

using Mapster;

namespace WebApi.Sales;

internal sealed class SaleMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSaleRequest, CreateSaleCommand>();

        config
            .NewConfig<(string SaleId, UpdateSaleRequest Request), UpdateSaleCommand>()
            .Map(dest => dest.SaleId, src => src.SaleId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<SaleResult, SaleResponse>();
    }
}
