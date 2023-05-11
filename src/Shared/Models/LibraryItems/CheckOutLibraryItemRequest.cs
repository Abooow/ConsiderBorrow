using System.ComponentModel.DataAnnotations;

namespace ConsiderBorrow.Shared.Models.LibraryItems;

public sealed class CheckOutLibraryItemRequest
{
    [Required]
    [MaxLength(100, ErrorMessage = "Customer name can not exceed 100 characters.")]
    public string CustomerName { get; set; } = default!;
}
