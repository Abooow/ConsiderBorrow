using ConsiderBorrow.Server.DataAccess;
using ConsiderBorrow.Shared.Models.Employees;
using ConsiderBorrow.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.Services;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(ApplicationDbContext dbContext, ILogger<EmployeeService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest createEmployeeRequest)
    {
        // Validate Role value.
        var roleValidationResult = ValidateEmployeeRole(createEmployeeRequest.Role);
        if (!roleValidationResult.Succeeded)
            return Result<EmployeeResponse>.CopyOf(roleValidationResult);

        // Ensure only one CEO can be present at a time.
        if (createEmployeeRequest.Role is EmployeeRole.CEO && await _dbContext.Employees.AnyAsync(x => x.IsCEO))
            return Result<EmployeeResponse>.Fail("There is already a CEO registered in the system.");

        var validationResult = await ValidateEmployeeRoleAndManagerRelationshipAsync(createEmployeeRequest.Role, createEmployeeRequest.ManagerId);
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

        _logger.LogInformation("Created new employee with ID {Id}", employeeRecord.Id);

        var response = CreateEmployeeResponse(employeeRecord, Array.Empty<int>());
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
                x.ManagerId,
                x.ManagedEmployees.Select(x => x.Id)))
            .FirstOrDefaultAsync();

        return employee is null
            ? Result<EmployeeResponse>.Fail($"Could not find an employee with ID {id}").AsNotFound()
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
                x.ManagerId,
                x.ManagedEmployees.Select(x => x.Id)))
            .ToListAsync();

        return employees;
    }

    public async Task<Result<EmployeeResponse>> UpdateEmployeeAsync(int id, UpdateEmployeeRequest updateEmployeeRequest)
    {
        var record = await _dbContext.Employees.FindAsync(id);
        if (record is null)
            return Result<EmployeeResponse>.Fail($"Could not find an employee with ID {id}").AsNotFound();

        record.FirstName = updateEmployeeRequest.FirstName ?? record.FirstName;
        record.LastName = updateEmployeeRequest.LastName ?? record.LastName;

        var employeeRole = record.IsCEO ? EmployeeRole.CEO : record.IsManager ? EmployeeRole.Manager : EmployeeRole.Employee;
        var salaryRank = updateEmployeeRequest.SalaryRank ?? record.Salary / SalaryCalculator.GetSalaryCoefficient(employeeRole);

        // Change Role.
        if (updateEmployeeRequest.Role is not null)
        {
            var roleValidationResult = ValidateEmployeeRole(updateEmployeeRequest.Role.Value);
            if (!roleValidationResult.Succeeded)
                return Result<EmployeeResponse>.CopyOf(roleValidationResult);

            // Ensure only one CEO can be present at a time.
            if (updateEmployeeRequest.Role is EmployeeRole.CEO && await _dbContext.Employees.AnyAsync(x => x.IsCEO))
                return Result<EmployeeResponse>.Fail("There is already a CEO registered in the system.");

            if (updateEmployeeRequest.Role is EmployeeRole.CEO && record.ManagerId is not null && updateEmployeeRequest.UpdateManager && updateEmployeeRequest.ManagerId is not null)
                return Result<EmployeeResponse>.Fail("Can not promote to CEO when being managed by someone.");

            if (updateEmployeeRequest.Role is EmployeeRole.Employee && await _dbContext.Employees.AnyAsync(x => x.ManagerId == id))
                return Result<EmployeeResponse>.Fail("Can not be demoted to regular employee when managing other employees.");

            employeeRole = updateEmployeeRequest.Role.Value;
            record.IsCEO = employeeRole is EmployeeRole.CEO;
            record.IsManager = employeeRole is EmployeeRole.Manager;
        }

        // Change to new Manager.
        if (updateEmployeeRequest.UpdateManager)
        {
            if (record.Id == updateEmployeeRequest.ManagerId)
                return Result<EmployeeResponse>.Fail("An employee cannot be their own manager.");

            record.ManagerId = updateEmployeeRequest.ManagerId;
        }

        // Validate employee role and manager relationship when changing role or manager.
        if (updateEmployeeRequest.Role is not null || updateEmployeeRequest.UpdateManager)
        {
            int? managerId = updateEmployeeRequest.UpdateManager ? updateEmployeeRequest.ManagerId : record.ManagerId;
            var validationResult = await ValidateEmployeeRoleAndManagerRelationshipAsync(employeeRole, managerId);
            if (!validationResult.Succeeded)
                return Result<EmployeeResponse>.CopyOf(validationResult);
        }

        record.Salary = SalaryCalculator.CalculateSalary(employeeRole, salaryRank);

        _dbContext.Employees.Update(record);
        await _dbContext.SaveChangesAsync();

        var managedEmployees = await _dbContext.Employees.Where(x => x.ManagerId == id).Select(x => x.Id).ToListAsync();

        _logger.LogInformation("Updated employee with ID {Id}", record.Id);

        var response = CreateEmployeeResponse(record, managedEmployees);
        return Result<EmployeeResponse>.Success(response);
    }

    public async Task<Result> DeleteEmployeeAsync(int id)
    {
        var record = await _dbContext.Employees.FindAsync(id);
        if (record is null)
            return Result.Fail($"Could not find an employee with ID {id}").AsNotFound();

        if (record.IsCEO || record.IsManager)
        {
            bool isManagingOthers = await _dbContext.Employees.AnyAsync(x => x.ManagerId == id);
            if (isManagingOthers)
                return Result.Fail("Cannot delete this employee. This employee is currently managing other employees.");
        }

        _dbContext.Employees.Remove(record);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Deleted employee with ID {Id}", record.Id);

        return Result.Success();
    }

    private static EmployeeResponse CreateEmployeeResponse(EmployeeRecord record, IEnumerable<int> managedEmployees)
    {
        return new EmployeeResponse(
            record.Id,
            record.FirstName,
            record.LastName,
            $"{record.FirstName} {record.LastName}",
            record.Salary,
            record.IsCEO ? EmployeeRole.CEO : record.IsManager ? EmployeeRole.Manager : EmployeeRole.Employee,
            record.ManagerId,
            managedEmployees);
    }

    private static Result ValidateEmployeeRole(EmployeeRole role)
    {
        if (role is < EmployeeRole.Employee or > EmployeeRole.CEO)
            return Result.Fail($"{role} is not a valid value for Role. Only Employee (0), Manager (1) and CEO (2) is allowed.").WithDescription(StatusCodeDescriptions.ValidationError);

        return Result.Success();
    }

    private async Task<Result> ValidateEmployeeRoleAndManagerRelationshipAsync(EmployeeRole role, int? managerId)
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
            return Result.Fail($"Could not set the manager because no manager with ID {managerId} exists.").AsNotFound();

        // Regular employees can not be managers.
        if (!managerRecord.IsCEO && !managerRecord.IsManager)
            return Result.Fail("Can not set a regular employee as a manager.");

        // CEO can not be a manager for Regular employees.
        if (role is EmployeeRole.Employee && managerRecord.IsCEO)
            return Result.Fail("Can not set the CEO as a manager for an regular employee.");

        return Result.Success();
    }
}
