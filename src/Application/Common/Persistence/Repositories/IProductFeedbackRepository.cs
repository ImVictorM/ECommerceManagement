using Application.ProductFeedback.DTOs;

using Domain.ProductFeedbackAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using SharedKernel.Interfaces;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for product feedback persistence operations.
/// </summary>
public interface IProductFeedbackRepository
    : IBaseRepository<DomainProductFeedback, ProductFeedbackId>
{
    /// <summary>
    /// Retrieves product feedback satisfying the specified specification.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A detailed product feedback result.</returns>
    Task<IEnumerable<ProductFeedbackResult>> GetProductFeedbackDetailedSatisfyingAsync(
        ISpecificationQuery<DomainProductFeedback> specification,
        CancellationToken cancellationToken = default
    );
}
