using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.ProductFeedback.DTOs.Results;

namespace Application.ProductFeedback.Queries.GetCustomerProductFeedback;

/// <summary>
/// Represents a query to retrieve customer product feedback.
/// </summary>
/// <param name="UserId">The customer id.</param>
[Authorize(policyType: typeof(SelfOrAdminPolicy<GetCustomerProductFeedbackQuery>))]
public record class GetCustomerProductFeedbackQuery(string UserId) :
    IUserSpecificResource,
    IRequestWithAuthorization<IEnumerable<ProductFeedbackResult>>;

