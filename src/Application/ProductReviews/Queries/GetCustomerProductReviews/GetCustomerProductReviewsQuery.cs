using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
using Application.ProductReviews.DTOs.Results;

namespace Application.ProductReviews.Queries.GetCustomerProductReviews;

/// <summary>
/// Represents a query to retrieve reviews from a customer.
/// </summary>
/// <param name="UserId">The customer identifier.</param>
[Authorize(policyType: typeof(SelfOrAdminPolicy<GetCustomerProductReviewsQuery>))]
public record class GetCustomerProductReviewsQuery(string UserId) :
    IUserSpecificResource,
    IRequestWithAuthorization<IReadOnlyList<ProductReviewResult>>;

