using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.ProductFeedback.Errors;

using Domain.ProductFeedbackAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.ProductFeedback.Commands.DeactivateCustomerProductFeedback;

internal sealed partial class DeactivateCustomerProductFeedbackCommandHandler
    : IRequestHandler<DeactivateCustomerProductFeedbackCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductFeedbackRepository _productFeedbackRepository;

    public DeactivateCustomerProductFeedbackCommandHandler(
        IProductFeedbackRepository productFeedbackRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeactivateCustomerProductFeedbackCommandHandler> logger
    )
    {
        _productFeedbackRepository = productFeedbackRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeactivateCustomerProductFeedbackCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingCustomerProductFeedbackDeactivation(request.FeedbackId);

        var feedbackId = ProductFeedbackId.Create(request.FeedbackId);

        var feedback = await _productFeedbackRepository.FindByIdAsync(
            feedbackId,
            cancellationToken
        );

        if (feedback == null)
        {
            LogFeedbackNotFound();

            throw new ProductFeedbackNotFoundException(feedbackId);
        }

        feedback.Deactivate();

        await _unitOfWork.SaveChangesAsync();

        LogCustomerProductFeedbackDeactivateSuccessfully();

        return Unit.Value;
    }
}
