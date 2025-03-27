using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.ProductReviews.Errors;

using Domain.ProductReviewAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ProductReviews.Commands.DeactivateCustomerProductReview;

internal sealed partial class DeactivateCustomerProductReviewCommandHandler
    : IRequestHandler<DeactivateCustomerProductReviewCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductReviewRepository _productReviewRepository;

    public DeactivateCustomerProductReviewCommandHandler(
        IProductReviewRepository productReviewRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeactivateCustomerProductReviewCommandHandler> logger
    )
    {
        _productReviewRepository = productReviewRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeactivateCustomerProductReviewCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerProductReviewDeactivation(request.ReviewId);

        var reviewId = ProductReviewId.Create(request.ReviewId);

        var review = await _productReviewRepository.FindByIdAsync(
            reviewId,
            cancellationToken
        );

        if (review == null)
        {
            LogReviewNotFound();

            throw new ProductReviewNotFoundException(reviewId);
        }

        review.Deactivate();

        await _unitOfWork.SaveChangesAsync();

        LogCustomerProductReviewDeactivateSuccessfully();

        return Unit.Value;
    }
}
