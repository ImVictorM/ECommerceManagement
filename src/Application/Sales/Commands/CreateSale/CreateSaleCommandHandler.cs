using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Common.DTOs.Results;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Sales.Commands.CreateSale;

internal sealed partial class CreateSaleCommandHandler
    : IRequestHandler<CreateSaleCommand, CreatedResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleEligibilityService _saleEligibilityService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleCommandHandler(
        ISaleRepository saleRepository,
        ISaleEligibilityService saleEligibilityService,
        IUnitOfWork unitOfWork,
        ILogger<CreateSaleCommandHandler> logger
    )
    {
        _saleRepository = saleRepository;
        _saleEligibilityService = saleEligibilityService;
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

        var sale = Sale.Create(
            request.Discount,
            categoriesOnSale,
            productsOnSale,
            productsExcludedFromSale
        );

        LogSaleCreated();

        await _saleEligibilityService.EnsureSaleProductsEligibilityAsync(
            sale,
            cancellationToken
        );

        LogSaleProductsIsEligible();

        await _saleRepository.AddAsync(sale);

        await _unitOfWork.SaveChangesAsync();

        var idCreatedSale = sale.Id.ToString();

        LogSaleCreatedAndSavedSuccessfully(idCreatedSale);

        return new CreatedResult(idCreatedSale);
    }
}
