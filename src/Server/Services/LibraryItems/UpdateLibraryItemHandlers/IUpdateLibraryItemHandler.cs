using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services.LibraryItems.UpdateLibraryItemHandlers;

internal interface IUpdateLibraryItemHandler
{
    string HandleForType { get; }

    Result HandleUpdate(LibraryItemRecord record, UpdateLibraryItemRequest updateLibraryItemRequest);
}
