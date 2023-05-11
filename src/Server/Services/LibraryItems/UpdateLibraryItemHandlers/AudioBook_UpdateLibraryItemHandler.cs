using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services.LibraryItems.UpdateLibraryItemHandlers;

internal sealed class AudioBook_UpdateLibraryItemHandler : IUpdateLibraryItemHandler
{
    public string HandleForType => LibraryItemTypes.AudioBook;

    public Result HandleUpdate(LibraryItemRecord record, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        var errors = new List<string>(1);

        record.Author = null;
        record.Pages = null;
        record.IsBorrowable = true;

        if (record.RunTimeMinutes is null && updateLibraryItemRequest.RunTimeMinutes is null)
            errors.Add("The RunTimeMinutes field is required for audio books.");
        record.RunTimeMinutes = updateLibraryItemRequest.RunTimeMinutes ?? record.RunTimeMinutes;

        return errors.Any()
            ? Result.Fail(errors).WithDescription(StatusCodeDescriptions.ValidationError)
            : Result.Success();
    }
}
