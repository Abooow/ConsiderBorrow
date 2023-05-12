using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.Categories;
using ConsiderBorrow.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.Services;

internal sealed class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ApplicationDbContext dbContext, ILogger<CategoryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
    {
        // Ensure the new category name is unique.
        bool categoryNameIsNotUnique = await _dbContext.Categories.AnyAsync(x => x.CategoryName == createCategoryRequest.Name);
        if (categoryNameIsNotUnique)
            return Result<CategoryResponse>.Fail($"A category with name '{createCategoryRequest.Name}' already exists.");

        var record = new CategoryRecord()
        {
            CategoryName = createCategoryRequest.Name
        };

        _dbContext.Categories.Add(record);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Created new category with ID {Id}", record.Id);

        return Result<CategoryResponse>.Success(new CategoryResponse(record.Id, record.CategoryName));
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync()
    {
        return await _dbContext.Categories
            .OrderBy(x => x.CategoryName)
            .Select(x => new CategoryResponse(x.Id, x.CategoryName))
            .ToListAsync();
    }

    public async Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest)
    {
        var record = await _dbContext.Categories.FindAsync(id);
        if (record is null)
            return Result<CategoryResponse>.Fail($"Could not find a category with ID {id}");

        // New name is the same as old name, do an early return.
        if (record.CategoryName == updateCategoryRequest.NewName)
            return Result<CategoryResponse>.Success();

        // Ensure the new category name is not used by another record.
        bool categoryNameIsUsed = await _dbContext.Categories.AnyAsync(x => x.CategoryName == updateCategoryRequest.NewName);
        if (categoryNameIsUsed)
            return Result<CategoryResponse>.Fail($"A category with name '{updateCategoryRequest.NewName}' already exists.");

        // Update only CategoryName.
        record.CategoryName = updateCategoryRequest.NewName;
        _dbContext.Entry(record).Property(x => x.CategoryName).IsModified = true;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Updated category with ID {Id}", record.Id);

        return Result<CategoryResponse>.Success(new CategoryResponse(record.Id, record.CategoryName));
    }

    public async Task<Result> DeleteCategoryAsync(int id)
    {
        bool categoryExists = await _dbContext.Categories.AnyAsync(x => x.Id == id);
        if (!categoryExists)
            return Result.Fail($"Could not find a category with ID {id}");

        bool hasReferencingLibraryItems = await _dbContext.LibraryItems.AnyAsync(x => x.CategoryId == id);
        if (hasReferencingLibraryItems)
            return Result.Fail("Cannot delete this category. There are still some library items referencing it.");

        // Create a new category record class with same id, start tracking it and then remove it. This is to avoid having to query the entire object from db.
        var record = new CategoryRecord()
        {
            Id = id,
            CategoryName = default!
        };
        _dbContext.Categories.Attach(record);
        _dbContext.Categories.Remove(record);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Deleted category with ID {Id}", record.Id);

        return Result.Success();
    }
}
