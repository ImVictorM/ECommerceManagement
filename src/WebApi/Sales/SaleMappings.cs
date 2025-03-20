using Application.Sales.Commands.CreateSale;

using Contracts.Sales;

using Mapster;

namespace WebApi.Sales;

internal sealed class SaleMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSaleRequest, CreateSaleCommand>();
    }
}
