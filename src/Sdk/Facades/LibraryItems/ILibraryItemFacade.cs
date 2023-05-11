using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Sdk.Facades;

public interface ILibraryItemFacade
{
    Task<Result<LibraryItemResponse>> CreateLibraryItemAsync(CreateLibraryItemRequest createLibraryItemRequest);
    Task<Result<LibraryItemResponse>> GetLibraryItemAsync(int id);
    Task<IEnumerable<LibraryItemResponse>> GetLibraryItemsAsync(int currentPage = 0, int pageSize = 16, bool sortByType = false);
    Task<Result> BorrowLibraryItemAsync(int id, BorrowLibraryItemRequest borrowLibraryItemRequest);
    Task<Result> ReturnLibraryItemAsync(int id);
    Task<Result> UpdateItemAsync(int id, UpdateLibraryItemRequest updateLibraryItemRequest);
    Task<Result> DeleteItemAsync(int id);
}
