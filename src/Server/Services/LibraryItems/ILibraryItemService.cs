using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services;

public interface ILibraryItemService
{
    Task<Result<LibraryItemResponse>> CreateBookAsync(CreateBookRequest createBookRequest);
    Task<Result<LibraryItemResponse>> CreateDvdAsync(CreateDvdRequest createDvdRequest);
    Task<Result<LibraryItemResponse>> CreateAudioBookAsync(CreateAudioBookRequest createAudioBookRequest);
    Task<Result<LibraryItemResponse>> CreateReferenceBookAsync(CreateReferenceBookRequest createReferenceBookRequest);
}
