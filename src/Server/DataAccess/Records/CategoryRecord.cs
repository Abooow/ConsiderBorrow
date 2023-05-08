using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.DataAccess;

[Table("Categories", Schema = "ResourceManagement")]
[Index(nameof(CategoryName), IsUnique = true)]
internal sealed class CategoryRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(30)]
    public required string CategoryName { get; set; }
}
