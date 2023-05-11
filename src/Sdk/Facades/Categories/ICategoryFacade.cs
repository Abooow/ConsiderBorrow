using ConsiderBorrow.Shared.Models.Categories;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Sdk.Facades;

public interface ICategoryFacade
{
    Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest);
    Task<IEnumerable<CategoryResponse>> GetCategoriesAsync();
    Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest);
    Task<Result> DeleteCategoryAsync(int id);
}
