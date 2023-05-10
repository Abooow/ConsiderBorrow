namespace ConsiderBorrow.Shared.Models.LibraryItems;

public sealed record LibraryItemResponse(
    int Id,
    string Category,
    int CategoryId,
    string Title,
    string? Author,
    int? Pages,
    int? RunTimeMinutes,
    bool IsBorrowable,
    bool HasBeenBorrowed,
    string? Borrower,
    DateTime? BorrowDate,
    string Type)
{
    public string TitleAcronym { get; set; } = default!;
};
