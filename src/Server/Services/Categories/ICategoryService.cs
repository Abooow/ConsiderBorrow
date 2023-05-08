using ConsiderBorrow.Shared.Models.Categories;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services;

public interface ICategoryService
{
    Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest);
    Task<IEnumerable<CategoryResponse>> GetCategoriesAsync();
    Task<Result> UpdateCategoryAsync(UpdateCategoryRequest updateCategoryRequest);
    Task<Result> DeleteCategoryAsync(int id);
}
