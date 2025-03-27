using Application.ProductReviews.Commands.LeaveProductReview;
using Application.ProductReviews.DTOs.Results;

using Contracts.ProductReviews;

using Mapster;

namespace WebApi.ProductReviews;

internal class ProductReviewMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductReviewUserResult, ProductReviewUserResponse>();

        config.NewConfig<ProductReviewResult, ProductReviewResponse>();

        config
            .NewConfig<
                (string ProductId, LeaveProductReviewRequest Request),
                LeaveProductReviewCommand
            >()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<ProductReviewDetailedResult, ProductReviewDetailedResponse>();
    }
}
