using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services.LibraryItems.UpdateLibraryItemHandlers;

internal sealed class ReferenceBook_UpdateLibraryItemHandler : IUpdateLibraryItemHandler
{
    public string HandleForType => LibraryItemTypes.ReferenceBook;

    public Result HandleUpdate(LibraryItemRecord record, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        var errors = new List<string>(2);

        record.RunTimeMinutes = null;
        record.IsBorrowable = false;

        if (record.Author is null && updateLibraryItemRequest.Author is null)
            errors.Add("The Author field is required for reference books.");
        record.Author ??= updateLibraryItemRequest.Author;

        if (record.Pages is null && updateLibraryItemRequest.Pages is null)
            errors.Add("The Pages field is required for reference books.");
        record.Pages ??= updateLibraryItemRequest.Pages;

        return errors.Any()
            ? Result.Fail(errors).WithDescription(StatusCodeDescriptions.ValidationError)
            : Result.Success();
    }
}
