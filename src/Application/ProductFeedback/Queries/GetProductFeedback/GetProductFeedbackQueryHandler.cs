using Application.ProductFeedback.DTOs.Results;
using Application.Common.Persistence.Repositories;

using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.Specifications;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ProductFeedback.Queries.GetProductFeedback;

internal sealed partial class GetProductFeedbackQueryHandler
    : IRequestHandler<GetProductFeedbackQuery, IEnumerable<ProductFeedbackResult>>
{
    private readonly IProductFeedbackRepository _productFeedbackRepository;

    public GetProductFeedbackQueryHandler(
        IProductFeedbackRepository productFeedbackRepository,
        ILogger<GetProductFeedbackQueryHandler> logger
    )
    {
        _productFeedbackRepository = productFeedbackRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductFeedbackResult>> Handle(
        GetProductFeedbackQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingProductFeedbackRetrieval(request.ProductId);

        var productId = ProductId.Create(request.ProductId);

        var specifications =
            new QueryActiveProductFeedback()
            .And(new QueryFeedbackForProduct(productId));

        LogProductFeedbackSpecifications(
            nameof(QueryActiveProductFeedback),
            nameof(QueryFeedbackForProduct)
        );

        var productFeedback = await _productFeedbackRepository
            .GetProductFeedbackDetailedSatisfyingAsync(
                specifications,
                cancellationToken
            );

        LogProductFeedbackRetrievedSuccessfully(productFeedback.Count);

        return productFeedback;
    }
}
