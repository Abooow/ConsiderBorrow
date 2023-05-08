using ConsiderBorrow.Shared.Models.Categories;

namespace ConsiderBorrow.Server.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetCategoriesAsync();
}
