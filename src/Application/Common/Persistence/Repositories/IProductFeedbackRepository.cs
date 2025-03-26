using Domain.ProductFeedbackAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using SharedKernel.Interfaces;
using Application.ProductFeedback.DTOs.Results;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for product feedback persistence operations.
/// </summary>
public interface IProductFeedbackRepository
    : IBaseRepository<DomainProductFeedback, ProductFeedbackId>
{
    /// <summary>
    /// Retrieves detailed product feedback satisfying the specified specification.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Detailed product feedback results.</returns>
    Task<
        IReadOnlyList<ProductFeedbackDetailedResult>
    > GetProductFeedbackDetailedSatisfyingAsync(
        ISpecificationQuery<DomainProductFeedback> specification,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves product feedback satisfying the specified specification.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Product feedback results.</returns>
    Task<IReadOnlyList<ProductFeedbackResult>> GetProductFeedbackSatisfyingAsync(
        ISpecificationQuery<DomainProductFeedback> specification,
        CancellationToken cancellationToken = default
    );
}
