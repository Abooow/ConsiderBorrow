using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services;

internal interface IUpdateLibraryItemManager
{
    Result UpdateItem(string type, LibraryItemRecord record, UpdateLibraryItemRequest updateLibraryItemRequest);
}
