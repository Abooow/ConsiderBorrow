using Azure;
using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.Employees;
using ConsiderBorrow.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.Services;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest createEmployeeRequest)
    {
        // Validate Role value.
        if (createEmployeeRequest.Role is < EmployeeRole.Employee or > EmployeeRole.CEO)
            return Result<EmployeeResponse>.Fail($"{createEmployeeRequest.Role} is not a valid value for Role. Only Employee (0), Manager (1) and CEO (2) is allowed.").WithDescription(StatusCodeDescriptions.ValidationError);

        // Ensure only one CEO can be present at a time.
        if (createEmployeeRequest.Role is EmployeeRole.CEO && await _dbContext.Employees.AnyAsync(x => x.IsCEO))
            return Result<EmployeeResponse>.Fail("There is already a CEO registered in the system.");

        var validationResult = await ValidateEmployeeRoleAndManagerAsync(createEmployeeRequest.Role, createEmployeeRequest.ManagerId);
        if (!validationResult.Succeeded)
            return Result<EmployeeResponse>.CopyOf(validationResult);

        var employeeRecord = new EmployeeRecord()
        {
            FirstName = createEmployeeRequest.FirstName,
            LastName = createEmployeeRequest.LastName,
            Salary = SalaryCalculator.CalculateSalary(createEmployeeRequest.Role, createEmployeeRequest.SalaryRank),
            IsCEO = createEmployeeRequest.Role is EmployeeRole.CEO,
            IsManager = createEmployeeRequest.Role is EmployeeRole.Manager,
            ManagerId = createEmployeeRequest.ManagerId
        };

        _dbContext.Employees.Add(employeeRecord);
        await _dbContext.SaveChangesAsync();

        var response = new EmployeeResponse(
            employeeRecord.Id,
            employeeRecord.FirstName,
            employeeRecord.LastName,
            $"{employeeRecord.FirstName} {employeeRecord.LastName}",
            employeeRecord.Salary,
            createEmployeeRequest.Role,
            employeeRecord.ManagerId);
        return Result<EmployeeResponse>.Success(response);
    }

    public async Task<Result<EmployeeResponse>> GetEmployeeAsync(int id)
    {
        var employee = await _dbContext
            .Employees
            .Where(x => x.Id == id)
            .Select(x => new EmployeeResponse(
                x.Id,
                x.FirstName,
                x.LastName,
                $"{x.FirstName} {x.LastName}",
                x.Salary,
                x.IsCEO ? EmployeeRole.CEO : x.IsManager ? EmployeeRole.Manager : EmployeeRole.Employee,
                x.ManagerId))
            .FirstOrDefaultAsync();

        return employee is null
            ? Result<EmployeeResponse>.Fail($"Could not find an employee with ID {id}")
            : Result<EmployeeResponse>.Success(employee);
    }

    public async Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync()
    {
        var employees = await _dbContext
            .Employees
            .OrderByDescending(x => x.IsCEO)
            .ThenByDescending(x => x.IsManager)
            .ThenBy(x => x.Id)
            .Select(x => new EmployeeResponse(
                x.Id,
                x.FirstName,
                x.LastName,
                $"{x.FirstName} {x.LastName}",
                x.Salary,
                x.IsCEO ? EmployeeRole.CEO : x.IsManager ? EmployeeRole.Manager : EmployeeRole.Employee,
                x.ManagerId))
            .ToListAsync();

        return employees;
    }

    public async Task<Result> DeleteEmployeeAsync(int id)
    {
        var record = await _dbContext.Employees.FindAsync(id);
        if (record is null)
            return Result.Fail($"Could not find an employee with ID {id}");

        if (record.IsCEO || record.IsManager)
        {
            bool isManagingOthers = await _dbContext.Employees.AnyAsync(x => x.ManagerId == id);
            if (isManagingOthers)
                return Result.Fail("Cannot delete this employee. This employee is currently managing other employees.");
        }

        _dbContext.Employees.Remove(record);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result> ValidateEmployeeRoleAndManagerAsync(EmployeeRole role, int? managerId)
    {
        // Ensure no one can be manager of the CEO.
        if (role is EmployeeRole.CEO && managerId is not null)
            return Result.Fail("CEO is not allowed to have a manager defined.");

        // Regular employees must have a manager defined.
        if (role is EmployeeRole.Employee && managerId is null)
            return Result.Fail("A manager must be defined for regular employees.");

        if (managerId is null)
            return Result.Success();

        var managerRecord = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == managerId);
        if (managerRecord is null)
            return Result.Fail($"Could not set the manager because no manager with ID {managerId} exists.");

        // When managerRecord is a regular employee.
        if (!managerRecord.IsCEO && !managerRecord.IsManager)
            return Result.Fail("Can not set a regular employee as a manager.");

        // When managerRecord is a CEO.
        if (role is EmployeeRole.Employee && managerRecord.IsCEO)
            return Result.Fail("Can not set the CEO as a manager for an regular employee.");

        return Result.Success();
    }
}
