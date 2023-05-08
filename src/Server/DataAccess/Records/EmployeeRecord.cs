using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsiderBorrow.Server.DataAccess;

[Table("Employees", Schema = "Identity")]
internal sealed class EmployeeRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    [Column(TypeName = "money")]
    public required decimal Salary { get; set; }

    public bool IsCEO { get; set; }

    public bool IsManager { get; set; }

    public int? ManagerId { get; set; }

    public EmployeeRecord? Manager { get; set; }
}
