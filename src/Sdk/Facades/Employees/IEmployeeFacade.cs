using ConsiderBorrow.Shared.Models.Employees;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Sdk.Facades;

public interface IEmployeeFacade
{
    Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest createEmployeeRequest);
    Task<Result<EmployeeResponse>> GetEmployeeAsync(int id);
    Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync();
    Task<Result<EmployeeResponse>> UpdateEmployeeAsync(int id, UpdateEmployeeRequest updateEmployeeRequest);
    Task<Result> DeleteEmployeeAsync(int id);
}
