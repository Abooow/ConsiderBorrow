using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsiderBorrow.Server.DataAccess;

[Table("LibraryItems", Schema = "ResourceManagement")]
internal sealed class LibraryItemRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public required int CategoryId { get; set; }

    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(100)]
    public string? Author { get; set; }

    public int? Pages { get; set; }

    public int? RunTimeMinutes { get; set; }

    public bool IsBorrowable { get; set; }

    [MaxLength(100)]
    public string? Borrower { get; set; }

    public DateTime? BorrowDate { get; set; }

    [MaxLength(20)]
    public required string Type { get; set; }

    public CategoryRecord? Category { get; set; }
}
