using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services;

public interface ILibraryItemService
{
    Task<Result<LibraryItemResponse>> CreateBookAsync(CreateBookRequest createBookRequest);
    Task<Result<LibraryItemResponse>> CreateDvdAsync(CreateDvdRequest createDvdRequest);
    Task<Result<LibraryItemResponse>> CreateAudioBookAsync(CreateAudioBookRequest createAudioBookRequest);
    Task<Result<LibraryItemResponse>> CreateReferenceBookAsync(CreateReferenceBookRequest createReferenceBookRequest);

    Task<IEnumerable<LibraryItemResponse>> GetLibraryItemsAsync(int currentPage, int pageSize, bool sortByType);

    Task<Result> BorrowItemAsync(int id, BorrowLibraryItemRequest borrowLibraryItemRequest);
    Task<Result> ReturnItemAsync(int id);

    Task<Result> UpdateItemAsync(int id, UpdateLibraryItemRequest updateLibraryItemRequest);
    Task<Result> DeleteItemAsync(int id);
}
