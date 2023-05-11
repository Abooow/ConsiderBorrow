using System.Text.Json;
using System.Text.Json.Serialization;
using ConsiderBorrow.Shared.Results;

namespace System.Net.Http;

public static class HttpResponseMessageExtensions
{
    public static async Task<Result> ToResultAsync(this HttpResponseMessage response)
    {
        try
        {
            string responseAsString = await response.Content.ReadAsStringAsync();
            Result responseObject = JsonSerializer.Deserialize<Result>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            })!;

            return responseObject;
        }
        catch (Exception)
        {
            return Result.Fail("Something went wrong.");
        }
    }

    public static async Task<Result<T>> ToResultAsync<T>(this HttpResponseMessage response)
    {
        try
        {
            string responseAsString = await response.Content.ReadAsStringAsync();
            Result<T> responseObject = JsonSerializer.Deserialize<Result<T>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            })!;

            return responseObject;
        }
        catch (Exception)
        {
            return Result<T>.Fail("Something went wrong.");
        }
    }
}
