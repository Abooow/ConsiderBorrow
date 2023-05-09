using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.Services;

internal sealed class LibraryItemService : ILibraryItemService
{
    private readonly ApplicationDbContext _dbContext;

    public LibraryItemService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
            libraryItemRecord.Type);
        return Result<LibraryItemResponse>.Success(response);
    }

    public async Task<IEnumerable<LibraryItemResponse>> GetLibraryItemsAsync(int currentPage, int pageSize, bool sortByType)
    {
        var query = sortByType
            ? _dbContext.LibraryItems.OrderBy(x => x.Type)
            : _dbContext.LibraryItems.OrderBy(x => x.Category!.CategoryName);

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
                x.BorrowDate == null,
                x.Borrower,
                x.BorrowDate,
                x.Type))
            .ToListAsync();

        return libraryItems;
    }
}
