using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services;

public interface ILibraryItemService
{
    Task<Result<LibraryItemResponse>> CreateLibraryItemAsync(CreateLibraryItemRequest createLibraryItemRequest);

    Task<Result<LibraryItemResponse>> GetLibraryItemAsync(int id);
    Task<IEnumerable<LibraryItemResponse>> GetLibraryItemsAsync(int currentPage, int pageSize, bool sortByType);

    Task<Result<LibraryItemResponse>> CheckOutItemAsync(int id, CheckOutLibraryItemRequest checkOutLibraryItemRequest);
    Task<Result<LibraryItemResponse>> ReturnItemAsync(int id);

    Task<Result<LibraryItemResponse>> UpdateItemAsync(int id, UpdateLibraryItemRequest updateLibraryItemRequest);
    Task<Result> DeleteItemAsync(int id);
}
