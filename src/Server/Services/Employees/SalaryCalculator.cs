using ConsiderBorrow.Shared.Models.Employees;

namespace ConsiderBorrow.Server.Services;

internal static class SalaryCalculator
{
    public static decimal CalculateSalary(EmployeeRole role, int rank)
    {
        var salaryCoefficient = role switch
        {
            EmployeeRole.Employee => 1.125m,
            EmployeeRole.Manager => 1.725m,
            EmployeeRole.CEO => 2.725m,
            _ => throw new Exception($"{role} is not a valid role. Unable to calculate salary.")
        };

        return rank * salaryCoefficient;
    }
}
