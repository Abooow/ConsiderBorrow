using System.Net;

namespace ConsiderBorrow.Shared.Results;

public static partial class ResultExtensions
{
    public static Result WithStatusCode(this Result result, HttpStatusCode httpStatusCode)
    {
        return result with { StatusCode = httpStatusCode };
    }

    public static Result AsBadRequest(this Result result)
    {
        return result.WithStatusCode(HttpStatusCode.BadRequest);
    }

    public static Result AsNotFound(this Result result)
    {
        return result.WithStatusCode(HttpStatusCode.NotFound);
    }

    public static Result AsUnauthorized(this Result result)
    {
        return result.WithStatusCode(HttpStatusCode.Unauthorized);
    }
}