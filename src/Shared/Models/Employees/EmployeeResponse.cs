namespace ConsiderBorrow.Shared.Models.Employees;

public sealed record EmployeeResponse(int Id, string FirstName, string LastName, string FullName, decimal Salary, EmployeeRole Role, int? ManagerId, IEnumerable<int> ManagedEmployees);
