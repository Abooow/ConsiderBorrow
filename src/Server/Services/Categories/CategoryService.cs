using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.Services;

internal sealed class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync()
    {
        return await _dbContext.Categories
            .OrderBy(x => x.CategoryName)
            .Select(x => new CategoryResponse(x.Id, x.CategoryName))
            .ToListAsync();
    }
}
