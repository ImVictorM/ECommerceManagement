using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Common.DTOs;

using Contracts.Categories;

using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Mapping configuration between category objects.
/// </summary>
public class CategoryMappingConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>();

        config.NewConfig<(string Id, UpdateCategoryRequest Request), UpdateCategoryCommand>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Request.Name);

        config.NewConfig<CategoryResult, CategoryResponse>();
    }
}
