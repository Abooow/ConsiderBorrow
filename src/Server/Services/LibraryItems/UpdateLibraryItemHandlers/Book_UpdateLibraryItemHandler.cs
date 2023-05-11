using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Server.Services.LibraryItems.UpdateLibraryItemHandlers;

internal sealed class Book_UpdateLibraryItemHandler : IUpdateLibraryItemHandler
{
    public string HandleForType => LibraryItemTypes.Book;

    public Result HandleUpdate(LibraryItemRecord record, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        var errors = new List<string>(2);

        record.RunTimeMinutes = null;
        record.IsBorrowable = true;

        if (record.Author is null && updateLibraryItemRequest.Author is null)
            errors.Add("The Author field is required for books.");
        record.Author = updateLibraryItemRequest.Author ?? record.Author;

        if (record.Pages is null && updateLibraryItemRequest.Pages is null)
            errors.Add("The Pages field is required for books.");
        record.Pages = updateLibraryItemRequest.Pages ?? record.Pages;

        return errors.Any()
            ? Result.Fail(errors).WithDescription(StatusCodeDescriptions.ValidationError)
            : Result.Success();
    }
}
