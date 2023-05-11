using ConsiderBorrow.Shared.Models.LibraryItems;

namespace ConsiderBorrow.Client.Shared;

public sealed record LibraryItemUpdated(LibraryItemResponse OldItem, LibraryItemResponse NewItem);
