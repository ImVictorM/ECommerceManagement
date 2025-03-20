using Application.Common.DTOs;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Factories;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Sales.Commands.CreateSale;

internal sealed partial class CreateSaleCommandHandler
    : IRequestHandler<CreateSaleCommand, CreatedResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly SaleFactory _saleFactory;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleCommandHandler(
        ISaleRepository saleRepository,
        ISaleEligibilityService saleEligibilityService,
        IUnitOfWork unitOfWork,
        ILogger<CreateSaleCommandHandler> logger
    )
    {
        _saleRepository = saleRepository;
        _saleFactory = new SaleFactory(saleEligibilityService);
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreatedResult> Handle(
        CreateSaleCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingSaleCreation();

        var categoriesOnSale = request.CategoryOnSaleIds
            .Select(CategoryId.Create)
            .Select(SaleCategory.Create);

        var productsOnSale = request.ProductOnSaleIds
            .Select(ProductId.Create)
            .Select(SaleProduct.Create);

        var productsExcludedFromSale = request.ProductExcludedFromSaleIds
            .Select(ProductId.Create)
            .Select(SaleProduct.Create);

        var sale = await _saleFactory.CreateSaleAsync(
            request.Discount,
            categoriesOnSale,
            productsOnSale,
            productsExcludedFromSale,
            cancellationToken
        );

        LogSaleCreated();

        await _saleRepository.AddAsync(sale);

        await _unitOfWork.SaveChangesAsync();

        var idCreatedSale = sale.Id.ToString();

        LogSaleCreatedAndSavedSuccessfully(idCreatedSale);

        return new CreatedResult(idCreatedSale);
    }
}
