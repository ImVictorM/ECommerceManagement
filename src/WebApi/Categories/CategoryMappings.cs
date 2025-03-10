using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.DTOs;

using Contracts.Categories;

using Mapster;

namespace WebApi.Categories;

internal sealed class CategoryMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>()
            .ConstructUsing(src => new CreateCategoryCommand(src.Name));

        config.NewConfig<(string Id, UpdateCategoryRequest Request), UpdateCategoryCommand>()
            .ConstructUsing(src => new UpdateCategoryCommand(src.Id, src.Request.Name));

        config.NewConfig<CategoryResult, CategoryResponse>();
    }
}
