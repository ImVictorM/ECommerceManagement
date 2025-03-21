using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Sales.Errors;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Sales.Commands.UpdateSale;

internal sealed partial class UpdateSaleCommandHandler
    : IRequestHandler<UpdateSaleCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleEligibilityService _saleEligibilityService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSaleCommandHandler(
        ISaleRepository saleRepository,
        ISaleEligibilityService saleEligibilityService,
        IUnitOfWork unitOfWork,
        ILogger<UpdateSaleCommandHandler> logger
    )
    {
        _saleRepository = saleRepository;
        _saleEligibilityService = saleEligibilityService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        UpdateSaleCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingSaleUpdate(request.SaleId);

        var saleId = SaleId.Create(request.SaleId);

        var sale = await _saleRepository.FindByIdAsync(saleId, cancellationToken);

        if (sale == null)
        {
            LogSaleNotFound();
            throw new SaleNotFoundException(saleId);
        }

        sale.Update(
            request.Discount,
            request.CategoryOnSaleIds
                .Select(CategoryId.Create)
                .Select(SaleCategory.Create),
            request.ProductOnSaleIds
                .Select(ProductId.Create)
                .Select(SaleProduct.Create),
            request.ProductExcludedFromSaleIds
                .Select(ProductId.Create)
                .Select(SaleProduct.Create)
        );

        LogSaleUpdated();

        await _saleEligibilityService.EnsureSaleProductsEligibilityAsync(
            sale,
            cancellationToken
        );

        LogSaleProductsIsEligible();

        await _unitOfWork.SaveChangesAsync();

        LogSaleUpdatedAndSavedSuccessfully();

        return Unit.Value;
    }
}
