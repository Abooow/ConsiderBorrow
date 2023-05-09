using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.Services;

internal sealed class LibraryItemService : ILibraryItemService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAcronymGenerator _acronymGenerator;

    public LibraryItemService(ApplicationDbContext dbContext, IAcronymGenerator acronymGenerator)
    {
        _dbContext = dbContext;
        _acronymGenerator = acronymGenerator;
    }

    public Task<Result<LibraryItemResponse>> CreateBookAsync(CreateBookRequest createBookRequest)
    {
        var record = new LibraryItemRecord()
        {
            CategoryId = createBookRequest.CategoryId,
            Title = createBookRequest.Title,
            Author = createBookRequest.Author,
            Pages = createBookRequest.Pages,
            IsBorrowable = true,
            Type = "Book"
        };

        return CreateLibraryItemAsync(record);
    }

    public Task<Result<LibraryItemResponse>> CreateDvdAsync(CreateDvdRequest createDvdRequest)
    {
        var record = new LibraryItemRecord()
        {
            CategoryId = createDvdRequest.CategoryId,
            Title = createDvdRequest.Title,
            RunTimeMinutes = createDvdRequest.RunTimeMinutes,
            IsBorrowable = true,
            Type = "DVD"
        };

        return CreateLibraryItemAsync(record);
    }

    public Task<Result<LibraryItemResponse>> CreateAudioBookAsync(CreateAudioBookRequest createAudioBookRequest)
    {
        var record = new LibraryItemRecord()
        {
            CategoryId = createAudioBookRequest.CategoryId,
            Title = createAudioBookRequest.Title,
            RunTimeMinutes = createAudioBookRequest.RunTimeMinutes,
            IsBorrowable = true,
            Type = "Audio Book"
        };

        return CreateLibraryItemAsync(record);
    }

    public Task<Result<LibraryItemResponse>> CreateReferenceBookAsync(CreateReferenceBookRequest createReferenceBookRequest)
    {
        var record = new LibraryItemRecord()
        {
            CategoryId = createReferenceBookRequest.CategoryId,
            Title = createReferenceBookRequest.Title,
            Author = createReferenceBookRequest.Author,
            Pages = createReferenceBookRequest.Pages,
            IsBorrowable = false,
            Type = "Reference Book"
        };

        return CreateLibraryItemAsync(record);
    }

    private async Task<Result<LibraryItemResponse>> CreateLibraryItemAsync(LibraryItemRecord libraryItemRecord)
    {
        // Ensure category exists.
        var category = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == libraryItemRecord.CategoryId);
        if (category is null)
            return Result<LibraryItemResponse>.Fail($"Could not find a category with ID {libraryItemRecord.CategoryId}");

        _dbContext.LibraryItems.Add(libraryItemRecord);
        await _dbContext.SaveChangesAsync();

        var response = new LibraryItemResponse(
            libraryItemRecord.Id,
            category.CategoryName,
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

    public async Task<Result> BorrowItemAsync(int itemId, BorrowLibraryItemRequest borrowLibraryItemRequest)
    {
        var record = await _dbContext.LibraryItems.FindAsync(itemId);
        if (record is null)
            return Result.Fail($"Could not find a library item with ID {itemId}");

        // Check if the customer can borrow this item.
        if (!record.IsBorrowable)
            return Result.Fail($"This item of type '{record.Type}' is not eligible for borrowing.");

        if (record.Borrower is not null)
            return Result.Fail("This item is already borrowed by another customer.");

        // Mark item as borrowed.
        record.Borrower = borrowLibraryItemRequest.CustomerName;
        _dbContext.Entry(record).Property(x => x.Borrower).IsModified = true;

        record.BorrowDate = DateTime.UtcNow;
        _dbContext.Entry(record).Property(x => x.BorrowDate).IsModified = true;

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ReturnItemAsync(int itemId)
    {
        var record = await _dbContext.LibraryItems.FindAsync(itemId);
        if (record is null)
            return Result.Fail($"Could not find a library item with ID {itemId}");

        // Check if item can be returned.
        if (!record.IsBorrowable)
            return Result.Fail($"This item of type '{record.Type}' is not eligible for returning.");

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

    public async Task<Result> DeleteItemAsync(int itemId)
    {
        var record = await _dbContext.LibraryItems.FindAsync(itemId);
        if (record is null)
            return Result.Fail($"Could not find a library item with ID {itemId}");

        if (record.Borrower is not null)
            return Result.Fail("Cannot delete this library item. There is someone that is borrowing it.");

        _dbContext.LibraryItems.Remove(record);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
