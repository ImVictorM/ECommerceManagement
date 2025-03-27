using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.DTOs.Results;

using Contracts.Categories;

using Mapster;

namespace WebApi.Categories;

internal sealed class CategoryMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>();

        config
            .NewConfig<(string Id, UpdateCategoryRequest Request), UpdateCategoryCommand>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Request.Name);

        config.NewConfig<CategoryResult, CategoryResponse>();
    }
}
