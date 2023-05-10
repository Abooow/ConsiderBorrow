using System.ComponentModel.DataAnnotations;

namespace ConsiderBorrow.Shared.Models.LibraryItems;

public sealed class UpdateLibraryItemRequest
{
    public int? CategoryId { get; set; }

    [MaxLength(100, ErrorMessage = "Title can not exceed 100 characters.")]
    public string? Title { get; set; }

    [MaxLength(100, ErrorMessage = "Author name can not exceed 100 characters.")]
    public string? Author { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Number of pages has to be at least 1.")]
    public int? Pages { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Runtime minutes has to be at least 1 minute.")]
    public int? RunTimeMinutes { get; set; }

    [MaxLength(20)]
    public string? Type { get; set; }
}
