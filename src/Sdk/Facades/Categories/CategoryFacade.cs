using System.Net.Http.Json;
using ConsiderBorrow.Shared.Models.Categories;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Sdk.Facades;

public sealed class CategoryFacade : ICategoryFacade
{
    private readonly HttpClient _httpClient;

    private const string baseUrl = "api/categories";

    public CategoryFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
    {
        var response = await _httpClient.PostAsJsonAsync(baseUrl, createCategoryRequest);
        return await response.ToResultAsync<CategoryResponse>();
    }

    public Task<IEnumerable<CategoryResponse>> GetCategoriesAsync()
    {
        return _httpClient.GetFromJsonAsync<IEnumerable<CategoryResponse>>(baseUrl)!;
    }

    public async Task<Result> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest)
    {
        var response = await _httpClient.PatchAsJsonAsync($"{baseUrl}/{id}", updateCategoryRequest);
        return await response.ToResultAsync();
    }

    public Task<Result> DeleteCategoryAsync(int id)
    {
        return _httpClient.DeleteFromJsonAsync<Result>($"{baseUrl}/{id}")!;
    }
}
