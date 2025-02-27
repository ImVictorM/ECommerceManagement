using Application.ProductFeedback.Commands.LeaveProductFeedback;

using Contracts.ProductFeedback;

using Mapster;

namespace WebApi.ProductFeedback;

internal class ProductFeedbackMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<(string ProductId, LeaveProductFeedbackRequest Request), LeaveProductFeedbackCommand>()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest, src => src.Request);
    }
}
