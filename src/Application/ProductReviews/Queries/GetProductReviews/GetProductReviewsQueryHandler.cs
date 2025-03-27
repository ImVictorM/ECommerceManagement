using Application.Common.Persistence.Repositories;
using Application.ProductReviews.DTOs.Results;

using Domain.ProductAggregate.ValueObjects;
using Domain.ProductReviewAggregate.Specifications;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ProductReviews.Queries.GetProductReviews;

internal sealed partial class GetProductReviewsQueryHandler
    : IRequestHandler<
        GetProductReviewsQuery,
        IReadOnlyList<ProductReviewDetailedResult>
      >

{
    private readonly IProductReviewRepository _productReviewRepository;

    public GetProductReviewsQueryHandler(
        IProductReviewRepository productReviewRepository,
        ILogger<GetProductReviewsQueryHandler> logger
    )
    {
        _productReviewRepository = productReviewRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductReviewDetailedResult>> Handle(
        GetProductReviewsQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingProductReviewsRetrieval(request.ProductId);

        var productId = ProductId.Create(request.ProductId);

        var specifications =
            new QueryActiveProductReviews()
            .And(new QueryProductReviews(productId));

        LogProductReviewsSpecifications(
            nameof(QueryActiveProductReviews),
            nameof(QueryProductReviews)
        );

        var reviews = await _productReviewRepository
            .GetProductReviewsDetailedSatisfyingAsync(
                specifications,
                cancellationToken
            );

        LogProductReviewsRetrievedSuccessfully(reviews.Count);

        return reviews
            .Select(ProductReviewDetailedResult.FromProjection)
            .ToList();
    }
}
