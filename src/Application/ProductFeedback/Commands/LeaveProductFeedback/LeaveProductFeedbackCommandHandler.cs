using Application.Common.DTOs;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Common.Security.Identity;
using Application.ProductFeedback.Errors;

using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.Services;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ProductFeedback.Commands.LeaveProductFeedback;

internal sealed partial class LeaveProductFeedbackCommandHandler
    : IRequestHandler<LeaveProductFeedbackCommand, CreatedResult>
{
    private readonly IProductFeedbackRepository _productFeedbackRepository;
    private readonly IProductFeedbackService _productFeedbackService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityProvider _identityProvider;

    public LeaveProductFeedbackCommandHandler(
        IProductFeedbackRepository productFeedbackRepository,
        IProductFeedbackService productFeedbackService,
        IUnitOfWork unitOfWork,
        IIdentityProvider identityProvider,
        ILogger<LeaveProductFeedbackCommandHandler> logger
    )
    {
        _productFeedbackRepository = productFeedbackRepository;
        _productFeedbackService = productFeedbackService;
        _unitOfWork = unitOfWork;
        _identityProvider = identityProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(
        LeaveProductFeedbackCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiateLeavingProductFeedback(request.ProductId);

        var currentUser = _identityProvider.GetCurrentUserIdentity();
        var currentUserId = UserId.Create(currentUser.Id);
        var productToLeaveFeedbackId = ProductId.Create(request.ProductId);

        LogCurrentUserLeavingFeedback(currentUser.Id);

        if (!await _productFeedbackService.CanLeaveFeedbackAsync(
            currentUserId,
            productToLeaveFeedbackId,
            cancellationToken
        ))
        {
            LogUserCannotLeaveFeedbackForProduct();

            throw new UserCannotLeaveFeedbackException();
        }

        LogUserAllowedToLeaveFeedback();

        var productFeedback = DomainProductFeedback.Create(
            currentUserId,
            productToLeaveFeedbackId,
            request.Title,
            request.Content,
            StarRating.Create(request.StarRating)
        );

        LogFeedbackCreated();

        await _productFeedbackRepository.AddAsync(productFeedback);

        await _unitOfWork.SaveChangesAsync();

        LogFeedbackSavedSuccessfully();

        return new CreatedResult(productFeedback.Id.ToString());
    }
}
