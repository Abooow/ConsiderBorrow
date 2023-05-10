using ConsiderBorrow.Server.Services;
using ConsiderBorrow.Shared.Models.Employees;
using ConsiderBorrow.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace ConsiderBorrow.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<ActionResult<Result<EmployeeResponse>>> CreateEmployee(CreateEmployeeRequest createEmployeeRequest)
    {
        var result = await _employeeService.CreateEmployeeAsync(createEmployeeRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result<EmployeeResponse>>> GetEmployee(int id)
    {
        var result = await _employeeService.GetEmployeeAsync(id);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet]
    public async Task<IEnumerable<EmployeeResponse>> GetEmployees()
    {
        return await _employeeService.GetEmployeesAsync();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Result<EmployeeResponse>>> UpdateEmployee(int id, UpdateEmployeeRequest updateEmployeeRequest)
    {
        var result = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result<EmployeeResponse>>> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
