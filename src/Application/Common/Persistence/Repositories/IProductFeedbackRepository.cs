using Domain.ProductFeedbackAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for product feedback persistence operations.
/// </summary>
public interface IProductFeedbackRepository : IBaseRepository<DomainProductFeedback, ProductFeedbackId>
{
}
