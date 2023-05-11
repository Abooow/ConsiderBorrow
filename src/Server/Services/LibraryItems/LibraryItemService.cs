using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.Services;

internal sealed class LibraryItemService : ILibraryItemService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAcronymGenerator _acronymGenerator;
    private readonly IUpdateLibraryItemManager _updateLibraryItemManager;

    public LibraryItemService(ApplicationDbContext dbContext, IAcronymGenerator acronymGenerator, IUpdateLibraryItemManager updateLibraryItemManager)
    {
        _dbContext = dbContext;
        _acronymGenerator = acronymGenerator;
        _updateLibraryItemManager = updateLibraryItemManager;
    }

    public async Task<Result<LibraryItemResponse>> CreateLibraryItemAsync(CreateLibraryItemRequest createLibraryItemRequest)
    {
        // Ensure category exists.
        var category = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == createLibraryItemRequest.CategoryId);
        if (category is null)
            return Result<LibraryItemResponse>.Fail($"Could not find a category with ID {createLibraryItemRequest.CategoryId}");

        var libraryItemRecord = new LibraryItemRecord()
        {
            CategoryId = createLibraryItemRequest.CategoryId!.Value,
            Title = createLibraryItemRequest.Title,
            Type = createLibraryItemRequest.Type
        };

        var updateModel = new UpdateLibraryItemRequest()
        {
            Author = createLibraryItemRequest.Author,
            Pages = createLibraryItemRequest.Pages,
            RunTimeMinutes = createLibraryItemRequest.RunTimeMinutes,
        };

        // Let UpdateLibraryItemManager set remaining properties depending on type.
        var updateResult = _updateLibraryItemManager.UpdateItem(libraryItemRecord.Type, libraryItemRecord, updateModel);
        if (!updateResult.Succeeded)
            return Result<LibraryItemResponse>.CopyOf(updateResult);

        _dbContext.LibraryItems.Add(libraryItemRecord);
        await _dbContext.SaveChangesAsync();

        var response = new LibraryItemResponse(
            libraryItemRecord.Id,
            category.CategoryName,
            category.Id,
            libraryItemRecord.Title,
            libraryItemRecord.Author,
            libraryItemRecord.Pages,
            libraryItemRecord.RunTimeMinutes,
            libraryItemRecord.IsBorrowable,
            HasBeenBorrowed: false,
            Borrower: null,
            BorrowDate: null,
            libraryItemRecord.Type)
        {
            TitleAcronym = _acronymGenerator.CreateAcronym(libraryItemRecord.Title)
        };
        return Result<LibraryItemResponse>.Success(response);
    }

    public async Task<Result<LibraryItemResponse>> GetLibraryItemAsync(int id)
    {
        var libraryItem = await _dbContext.LibraryItems
            .Where(x => x.Id == id)
            .Select(x => new LibraryItemResponse(
                x.Id,
                x.Category!.CategoryName,
                x.CategoryId,
                x.Title,
                x.Author,
                x.Pages,
                x.RunTimeMinutes,
                x.IsBorrowable,
                x.Borrower != null,
                x.Borrower,
                x.BorrowDate,
                x.Type))
            .FirstOrDefaultAsync();

        if (libraryItem is null)
            return Result<LibraryItemResponse>.Fail($"Could not find a library item with ID {id}");

        libraryItem.TitleAcronym = _acronymGenerator.CreateAcronym(libraryItem.Title);

        return Result<LibraryItemResponse>.Success(libraryItem);
    }

    public async Task<IEnumerable<LibraryItemResponse>> GetLibraryItemsAsync(int currentPage, int pageSize, bool sortByType)
    {
        // Order by CategoryName (ASC) by default, else by Type (ASC).
        var query = sortByType
            ? _dbContext.LibraryItems.OrderBy(x => x.Type)
            : _dbContext.LibraryItems.OrderBy(x => x.Category!.CategoryName);

        // Using pagination to avoid returning all records.
        var libraryItems = await query
            .ThenBy(x => x.Id)
            .Skip(currentPage * pageSize)
            .Take(pageSize)
            .Select(x => new LibraryItemResponse(
                x.Id,
                x.Category!.CategoryName,
                x.CategoryId,
                x.Title,
                x.Author,
                x.Pages,
                x.RunTimeMinutes,
                x.IsBorrowable,
                x.Borrower != null,
                x.Borrower,
                x.BorrowDate,
                x.Type))
            .ToListAsync();

        // Generate an acronym based on title for every item. This can not be done as a SQL query if differed implementations is wanted.
        foreach (var item in libraryItems)
        {
            item.TitleAcronym = _acronymGenerator.CreateAcronym(item.Title);
        }

        return libraryItems;
    }

    public async Task<Result> CheckOutItemAsync(int id, CheckOutLibraryItemRequest checkOutLibraryItemRequest)
    {
        var record = await _dbContext.LibraryItems.FindAsync(id);
        if (record is null)
            return Result.Fail($"Could not find a library item with ID {id}");

        // Check if the customer can borrow this item.
        if (!record.IsBorrowable)
            return Result.Fail($"This item of type '{record.Type}' cannot be borrowed.");

        if (record.Borrower is not null)
            return Result.Fail("This item is already checked out by another customer.");

        // Mark item as borrowed.
        record.Borrower = checkOutLibraryItemRequest.CustomerName;
        _dbContext.Entry(record).Property(x => x.Borrower).IsModified = true;

        record.BorrowDate = DateTime.UtcNow;
        _dbContext.Entry(record).Property(x => x.BorrowDate).IsModified = true;

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ReturnItemAsync(int id)
    {
        var record = await _dbContext.LibraryItems.FindAsync(id);
        if (record is null)
            return Result.Fail($"Could not find a library item with ID {id}");

        // Check if item can be returned.
        if (!record.IsBorrowable)
            return Result.Fail($"This item of type '{record.Type}' cannot be returned.");

        if (record.Borrower is null)
            return Result.Fail("This item cannot be returned as it's not yet checked out.");

        // Mark item as returned.
        record.Borrower = null;
        _dbContext.Entry(record).Property(x => x.Borrower).IsModified = true;

        record.BorrowDate = null;
        _dbContext.Entry(record).Property(x => x.BorrowDate).IsModified = true;

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateItemAsync(int id, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        var record = await _dbContext.LibraryItems.FindAsync(id);
        if (record is null)
            return Result.Fail($"Could not find a library item with ID {id}");

        // Don't allow updates if someone is borrowing it.
        if (record.Borrower is not null)
            return Result.Fail("Cannot update this library item. There is someone that is borrowing it.");

        // Try update category.
        if (updateLibraryItemRequest.CategoryId is not null)
        {
            bool categoryExists = await _dbContext.Categories.AnyAsync(x => x.Id == updateLibraryItemRequest.CategoryId);
            if (!categoryExists)
                return Result.Fail($"Could not find a category with ID {id}");

            record.CategoryId = updateLibraryItemRequest.CategoryId.Value;
        }

        // Try update title.
        if (updateLibraryItemRequest.Title is not null)
            record.Title = updateLibraryItemRequest.Title;

        // Let UpdateLibraryItemManager update remaining properties depending on type.
        var updateResult = _updateLibraryItemManager.UpdateItem(updateLibraryItemRequest.Type ?? record.Type, record, updateLibraryItemRequest);
        if (!updateResult.Succeeded)
            return updateResult;

        record.Type = updateLibraryItemRequest.Type ?? record.Type;

        _dbContext.LibraryItems.Update(record);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteItemAsync(int id)
    {
        var record = await _dbContext.LibraryItems.FindAsync(id);
        if (record is null)
            return Result.Fail($"Could not find a library item with ID {id}");

        if (record.Borrower is not null)
            return Result.Fail("Cannot delete this library item. There is someone that is borrowing it.");

        _dbContext.LibraryItems.Remove(record);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
