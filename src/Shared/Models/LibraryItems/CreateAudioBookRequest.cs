using System.ComponentModel.DataAnnotations;

namespace ConsiderBorrow.Shared.Models.LibraryItems;

public sealed class CreateAudioBookRequest
{
    [Required]
    [MaxLength(100, ErrorMessage = "Title can not exceed 100 characters.")]
    public string Title { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "Runtime minutes has to be at least 1 minute.")]
    public int RunTimeMinutes { get; set; }

    public int CategoryId { get; set; }
}
