using ConsiderBorrow.Shared.Models.Employees;

namespace ConsiderBorrow.Server.Services;

internal static class SalaryCalculator
{
    public static decimal GetSalaryCoefficient(EmployeeRole role)
    {
        return role switch
        {
            EmployeeRole.Employee => 1.125m,
            EmployeeRole.Manager => 1.725m,
            EmployeeRole.CEO => 2.725m,
            _ => throw new Exception($"{role} is not a valid role.")
        };
    }

    public static decimal CalculateSalary(EmployeeRole role, decimal rank)
    {
        decimal salaryCoefficient = GetSalaryCoefficient(role);
        return rank * salaryCoefficient;
    }
}
