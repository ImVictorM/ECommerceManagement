using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Common.Security.Identity;
using Application.Common.DTOs.Results;
using Application.ProductReviews.Errors;

using Domain.ProductReviewAggregate.Services;
using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductReviewAggregate;
using Domain.UserAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ProductReviews.Commands.LeaveProductReview;

internal sealed partial class LeaveProductReviewCommandHandler
    : IRequestHandler<LeaveProductReviewCommand, CreatedResult>
{
    private readonly IProductReviewRepository _productReviewRepository;
    private readonly IProductReviewEligibilityService _eligibilityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityProvider _identityProvider;

    public LeaveProductReviewCommandHandler(
        IProductReviewRepository productReviewRepository,
        IProductReviewEligibilityService eligibilityService,
        IUnitOfWork unitOfWork,
        IIdentityProvider identityProvider,
        ILogger<LeaveProductReviewCommandHandler> logger
    )
    {
        _productReviewRepository = productReviewRepository;
        _eligibilityService = eligibilityService;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
        _logger = logger;
    }

    public async Task<CreatedResult> Handle(
        LeaveProductReviewCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiateLeavingProductReview(request.ProductId);

        var currentUser = _identityProvider.GetCurrentUserIdentity();
        var currentUserId = UserId.Create(currentUser.Id);
        var productToLeaveReviewId = ProductId.Create(request.ProductId);

        LogCurrentUserLeavingReview(currentUser.Id);

        if (!await _eligibilityService.CanLeaveReviewAsync(
            currentUserId,
            productToLeaveReviewId,
            cancellationToken
        ))
        {
            LogUserCannotLeaveReviewForProduct();

            throw new UserCannotLeaveReviewException();
        }

        LogUserAllowedToLeaveReview();

        var review = ProductReview.Create(
            currentUserId,
            productToLeaveReviewId,
            request.Title,
            request.Content,
            StarRating.Create(request.StarRating)
        );

        LogReviewCreated();

        await _productReviewRepository.AddAsync(review);

        await _unitOfWork.SaveChangesAsync();

        LogReviewSavedSuccessfully();

        return CreatedResult.FromId(review.Id.ToString());
    }
}
