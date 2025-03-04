using Application.ProductFeedback.Commands.LeaveProductFeedback;
using Application.ProductFeedback.DTOs;

using Contracts.ProductFeedback;

using Mapster;

namespace WebApi.ProductFeedback;

internal class ProductFeedbackMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ProductFeedbackUserResult, ProductFeedbackUserResponse>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Name, src => src.Name);

        config
            .NewConfig<ProductFeedbackResult, ProductFeedbackResponse>()
            .Map(dest => dest, src => src.ProductFeedback)
            .Map(dest => dest.Id, src => src.ProductFeedback.Id.ToString())
            .Map(dest => dest.ProductId, src => src.ProductFeedback.ProductId.ToString())
            .Map(dest => dest.Title, src => src.ProductFeedback.Title)
            .Map(dest => dest.Content, src => src.ProductFeedback.Content)
            .Map(dest => dest.StarRating, src => src.ProductFeedback.StarRating.Value)
            .Map(dest => dest.CreatedAt, src => src.ProductFeedback.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.ProductFeedback.UpdatedAt);

        config
            .NewConfig<
                (string ProductId, LeaveProductFeedbackRequest Request),
                LeaveProductFeedbackCommand
            >()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest, src => src.Request);

        config
            .NewConfig<ProductFeedbackDetailedResult, ProductFeedbackDetailedResponse>()
            .Map(dest => dest.Id, src => src.ProductFeedback.Id.ToString())
            .Map(dest => dest.Title, src => src.ProductFeedback.Title)
            .Map(dest => dest.Content, src => src.ProductFeedback.Content)
            .Map(dest => dest.StarRating, src => src.ProductFeedback.StarRating.Value)
            .Map(dest => dest.CreatedAt, src => src.ProductFeedback.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.ProductFeedback.UpdatedAt)
            .Map(dest => dest.User, src => src.User);
    }
}
