using Application.Common.Persistence.Repositories;
using Application.ProductReviews.DTOs.Results;

using Domain.UserAggregate.ValueObjects;
using Domain.ProductReviewAggregate.Specifications;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ProductReviews.Queries.GetCustomerProductReviews;

internal sealed partial class GetCustomerProductReviewsQueryHandler
    : IRequestHandler<
        GetCustomerProductReviewsQuery,
        IReadOnlyList<ProductReviewResult>
     >
{
    private readonly IProductReviewRepository _productReviewRepository;

    public GetCustomerProductReviewsQueryHandler(
        IProductReviewRepository productReviewRepository,
        ILogger<GetCustomerProductReviewsQueryHandler> logger
    )
    {
        _productReviewRepository = productReviewRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductReviewResult>> Handle(
        GetCustomerProductReviewsQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerProductReviewsRetrieval(request.UserId);

        var customerId = UserId.Create(request.UserId);

        var specifications =
            new QueryActiveProductReviews()
            .And(new QueryUserReviews(customerId));

        LogSpecifications(
            nameof(QueryActiveProductReviews),
            nameof(QueryUserReviews)
        );

        var reviews = await _productReviewRepository.FindSatisfyingAsync(
            specifications,
            cancellationToken
        );

        var result = reviews.Select(ProductReviewResult.FromReview).ToList();

        LogCustomerProductReviewsRetrievedSuccessfully(result.Count);

        return result;
    }
}
