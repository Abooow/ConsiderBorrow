using System.ComponentModel.DataAnnotations;

namespace ConsiderBorrow.Shared.Models.Employees;

public sealed class CreateEmployeeRequest
{
    [Required]
    [MaxLength(50, ErrorMessage = "FirstName can not exceed 50 characters.")]
    public string FirstName { get; set; } = default!;

    [Required]
    [MaxLength(50, ErrorMessage = "FirstName can not exceed 50 characters.")]
    public string LastName { get; set; } = default!;

    [Range(1, 10)]
    public int SalaryRank { get; set; }

    public EmployeeRole Role { get; set; }

    public int? ManagerId { get; set; }
}
