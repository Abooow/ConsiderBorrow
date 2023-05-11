using System.Net.Http.Json;
using ConsiderBorrow.Shared.Models.LibraryItems;
using ConsiderBorrow.Shared.Results;

namespace ConsiderBorrow.Sdk.Facades;

public sealed class LibraryItemFacade : ILibraryItemFacade
{
    private readonly HttpClient _httpClient;

    private const string baseUrl = "api/libraryitems";

    public LibraryItemFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<LibraryItemResponse>> CreateLibraryItemAsync(CreateLibraryItemRequest createLibraryItemRequest)
    {
        var response = await _httpClient.PostAsJsonAsync(baseUrl, createLibraryItemRequest);
        return await response.ToResultAsync<LibraryItemResponse>();
    }

    public Task<Result<LibraryItemResponse>> GetLibraryItemAsync(int id)
    {
        return _httpClient.GetFromJsonAsync<Result<LibraryItemResponse>>($"{baseUrl}/{id}")!;
    }

    public Task<IEnumerable<LibraryItemResponse>> GetLibraryItemsAsync(int currentPage = 0, int pageSize = 16, bool sortByType = false)
    {
        return _httpClient.GetFromJsonAsync<IEnumerable<LibraryItemResponse>>($"{baseUrl}?currentPage={currentPage}&pageSize={pageSize}&sortByType={sortByType}")!;
    }

    public async Task<Result> CheckOutLibraryItemAsync(int id, CheckOutLibraryItemRequest checkOutLibraryItemRequest)
    {
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/check-out/{id}", checkOutLibraryItemRequest);
        return await response.ToResultAsync();
    }

    public async Task<Result> ReturnLibraryItemAsync(int id)
    {
        var response = await _httpClient.PostAsync($"{baseUrl}/return/{id}", null);
        return await response.ToResultAsync();
    }

    public async Task<Result> UpdateItemAsync(int id, UpdateLibraryItemRequest updateLibraryItemRequest)
    {
        var response = await _httpClient.PatchAsJsonAsync($"{baseUrl}/{id}", updateLibraryItemRequest);
        return await response.ToResultAsync();
    }

    public Task<Result> DeleteItemAsync(int id)
    {
        return _httpClient.DeleteFromJsonAsync<Result>($"{baseUrl}/{id}")!;
    }
}
