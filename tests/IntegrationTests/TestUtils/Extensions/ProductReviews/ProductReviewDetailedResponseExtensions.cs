using Domain.UserAggregate.ValueObjects;
using Domain.ProductReviewAggregate;

using Contracts.ProductReviews;

using IntegrationTests.Common.Seeds.Users;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.ProductReviews;

/// <summary>
/// Extension methods for the <see cref="ProductReviewDetailedResponse"/> class.
/// </summary>
public static class ProductReviewDetailedResponseExtensions
{
    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product review.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="review">The expected product review.</param>
    /// <param name="userSeed">The user seed.</param>
    public static void EnsureCorrespondsTo(
        this ProductReviewDetailedResponse response,
        ProductReview review,
        IUserSeed userSeed
    )
    {
        response.Id.Should().Be(review.Id.ToString());
        response.Title.Should().Be(review.Title);
        response.Content.Should().Be(review.Content);
        response.StarRating.Should().Be(review.StarRating.Value);
        response.CreatedAt.Should().Be(review.CreatedAt);
        response.UpdatedAt.Should().Be(review.UpdatedAt);

        response.User.Id.Should().Be(review.UserId.ToString());
        response.User.Name.Should().Be(GetUserName(userSeed, review.UserId));
    }

    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product reviews.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="reviews">
    /// The expected product reviews.
    /// </param>
    /// <param name="userSeed">The user seed.</param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<ProductReviewDetailedResponse> response,
        IEnumerable<ProductReview> reviews,
        IUserSeed userSeed
    )
    {
        var expectedReviewList = reviews.ToList();
        var responseList = response.ToList();

        responseList.Count.Should().Be(expectedReviewList.Count);

        var expectedReviewsMap = expectedReviewList
            .ToDictionary(f => f.Id.ToString());

        foreach (var responseReview in responseList)
        {
            var currentExpectedProductReview = expectedReviewsMap
                [responseReview.Id];

            responseReview.EnsureCorrespondsTo(
                currentExpectedProductReview,
                userSeed
            );
        }
    }

    private static string GetUserName(IUserSeed seed, UserId userId)
    {
        return seed.ListAll(u => u.Id == userId)[0].Name;
    }
}
