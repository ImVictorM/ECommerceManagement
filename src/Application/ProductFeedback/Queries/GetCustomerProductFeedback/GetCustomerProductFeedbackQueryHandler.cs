using Application.Common.Persistence.Repositories;

using Domain.ProductFeedbackAggregate.Specifications;
using Domain.UserAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;
using Application.ProductFeedback.DTOs.Results;

namespace Application.ProductFeedback.Queries.GetCustomerProductFeedback;

internal sealed partial class GetCustomerProductFeedbackQueryHandler
    : IRequestHandler<GetCustomerProductFeedbackQuery, IEnumerable<ProductFeedbackResult>>
{
    private readonly IProductFeedbackRepository _productFeedbackRepository;

    public GetCustomerProductFeedbackQueryHandler(
        IProductFeedbackRepository productFeedbackRepository,
        ILogger<GetCustomerProductFeedbackQueryHandler> logger
    )
    {
        _productFeedbackRepository = productFeedbackRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductFeedbackResult>> Handle(
        GetCustomerProductFeedbackQuery request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerProductFeedbackRetrieval(request.UserId);

        var customerId = UserId.Create(request.UserId);

        var specifications = new QueryActiveProductFeedback()
            .And(new QueryUserProductFeedback(customerId));

        LogSpecifications(
            nameof(QueryActiveProductFeedback),
            nameof(QueryUserProductFeedback)
        );

        var customerProductFeedback = await _productFeedbackRepository
            .GetProductFeedbackSatisfyingAsync(specifications, cancellationToken);

        LogCustomerProductFeedbackRetrievedSuccessfully(
            customerProductFeedback.Count()
        );

        return customerProductFeedback;
    }
}
