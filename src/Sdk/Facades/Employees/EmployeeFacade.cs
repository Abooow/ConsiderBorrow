using System.Net.Http.Json;
using ConsiderBorrow.Shared.Models.Employees;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Sdk.Facades;

public sealed class EmployeeFacade : IEmployeeFacade
{
    private readonly HttpClient _httpClient;

    private const string baseUrl = "api/employees";

    public EmployeeFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest createEmployeeRequest)
    {
        var response = await _httpClient.PostAsJsonAsync(baseUrl, createEmployeeRequest);
        return await response.ToResultAsync<EmployeeResponse>();
    }

    public Task<Result<EmployeeResponse>> GetEmployeeAsync(int id)
    {
        return _httpClient.GetFromJsonAsync<Result<EmployeeResponse>>($"{baseUrl}/{id}")!;
    }

    public Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync()
    {
        return _httpClient.GetFromJsonAsync<IEnumerable<EmployeeResponse>>(baseUrl)!;
    }

    public async Task<Result> UpdateEmployeeAsync(int id, UpdateEmployeeRequest updateEmployeeRequest)
    {
        var response = await _httpClient.PatchAsJsonAsync($"{baseUrl}/{id}", updateEmployeeRequest);
        return await response.ToResultAsync();
    }

    public Task<Result> DeleteEmployeeAsync(int id)
    {
        return _httpClient.DeleteFromJsonAsync<Result>($"{baseUrl}/{id}")!;
    }
}
