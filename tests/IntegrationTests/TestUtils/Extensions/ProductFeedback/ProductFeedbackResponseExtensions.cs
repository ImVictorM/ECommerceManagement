using Contracts.ProductFeedback;

using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;
using Domain.UserAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Users;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.ProductFeedback;

/// <summary>
/// Extension methods for the <see cref="ProductFeedbackDetailedResponse"/> class.
/// </summary>
public static class ProductFeedbackResponseExtensions
{
    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product feedback.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="feedback">The expected product feedback.</param>
    /// <param name="userSeed">The user seed.</param>
    public static void EnsureCorrespondsTo(
        this ProductFeedbackDetailedResponse response,
        DomainProductFeedback feedback,
        IUserSeed userSeed
    )
    {
        response.Id.Should().Be(feedback.Id.ToString());
        response.Title.Should().Be(feedback.Title);
        response.Content.Should().Be(feedback.Content);
        response.StarRating.Should().Be(feedback.StarRating.Value);
        response.CreatedAt.Should().Be(feedback.CreatedAt);
        response.UpdatedAt.Should().Be(feedback.UpdatedAt);

        response.User.Id.Should().Be(feedback.UserId.ToString());
        response.User.Name.Should().Be(GetUserName(userSeed, feedback.UserId));
    }

    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product feedback.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="expectedProductFeedback">
    /// The expected product feedback.
    /// </param>
    /// <param name="userSeed">The user seed.</param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<ProductFeedbackDetailedResponse> response,
        IEnumerable<DomainProductFeedback> expectedProductFeedback,
        IUserSeed userSeed
    )
    {
        var expectedFeedbackItemsList = expectedProductFeedback.ToList();
        var responseList = response.ToList();

        responseList.Count.Should().Be(expectedFeedbackItemsList.Count);

        var expectedFeedbackItemsMap = expectedFeedbackItemsList
            .ToDictionary(f => f.Id.ToString());

        foreach (var productFeedbackResponse in responseList)
        {
            var currentExpectedProductFeedback = expectedFeedbackItemsMap
                [productFeedbackResponse.Id];

            productFeedbackResponse.EnsureCorrespondsTo(
                currentExpectedProductFeedback,
                userSeed
            );
        }
    }

    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product feedback.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="feedback">The expected product feedback.</param>
    public static void EnsureCorrespondsTo(
        this ProductFeedbackResponse response,
        DomainProductFeedback feedback
    )
    {
        response.Id.Should().Be(feedback.Id.ToString());
        response.ProductId.Should().Be(feedback.ProductId.ToString());
        response.Title.Should().Be(feedback.Title);
        response.Content.Should().Be(feedback.Content);
        response.StarRating.Should().Be(feedback.StarRating.Value);
        response.CreatedAt.Should().Be(feedback.CreatedAt);
        response.UpdatedAt.Should().Be(feedback.UpdatedAt);
    }

    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product feedback.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="expectedProductFeedback">
    /// The expected product feedback.
    /// </param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<ProductFeedbackResponse> response,
        IEnumerable<DomainProductFeedback> expectedProductFeedback
    )
    {
        var expectedFeedbackItemsList = expectedProductFeedback.ToList();
        var responseList = response.ToList();

        responseList.Count.Should().Be(expectedFeedbackItemsList.Count);

        var expectedFeedbackItemsMap = expectedFeedbackItemsList
            .ToDictionary(f => f.Id.ToString());

        foreach (var productFeedbackResponse in responseList)
        {
            var currentExpectedProductFeedback = expectedFeedbackItemsMap
                [productFeedbackResponse.Id];

            productFeedbackResponse.EnsureCorrespondsTo(
                currentExpectedProductFeedback
            );
        }
    }

    private static string GetUserName(IUserSeed seed, UserId userId)
    {
        return seed.ListAll(u => u.Id == userId)[0].Name;
    }
}
