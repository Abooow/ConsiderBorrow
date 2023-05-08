using System.Net;

namespace ConsiderBorrow.Shared.Results;

public static partial class ResultExtensions
{
    public static Result<T> WithStatusCode<T>(this Result<T> result, HttpStatusCode httpStatusCode)
    {
        return result with { StatusCode = httpStatusCode };
    }

    public static Result<T> AsBadRequest<T>(this Result<T> result)
    {
        return result.WithStatusCode(HttpStatusCode.BadRequest);
    }

    public static Result<T> AsNotFound<T>(this Result<T> result)
    {
        return result.WithStatusCode(HttpStatusCode.NotFound);
    }

    public static Result<T> AsUnauthorized<T>(this Result<T> result)
    {
        return result.WithStatusCode(HttpStatusCode.Unauthorized);
    }
}