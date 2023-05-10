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
    public async Task<ActionResult<Result>> CreateEmployee(CreateEmployeeRequest createEmployeeRequest)
    {
        var result = await _employeeService.CreateEmployeeAsync(createEmployeeRequest);

        return result.Succeeded
            ? Ok(result)
            : BadRequest(result);
    }
}
