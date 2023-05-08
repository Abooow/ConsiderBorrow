using System.ComponentModel.DataAnnotations;

namespace ConsiderBorrow.Shared.Models.Categories;

public sealed class CreateCategoryRequest
{
    [Required]
    [MaxLength(30, ErrorMessage = "Category Name can not exceed 30 characters.")]
    public required string Name { get; init; }
}
