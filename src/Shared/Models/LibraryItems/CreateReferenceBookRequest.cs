using System.ComponentModel.DataAnnotations;

namespace ConsiderBorrow.Shared.Models.LibraryItems;

public sealed class CreateReferenceBookRequest
{
    [Required]
    [MaxLength(100, ErrorMessage = "Title can not exceed 100 characters.")]
    public string Title { get; set; } = default!;

    [Required]
    [MaxLength(100, ErrorMessage = "Author name can not exceed 100 characters.")]
    public string Author { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "Reference book requires at least 1 page.")]
    public int Pages { get; set; }

    public int CategoryId { get; set; }
}
