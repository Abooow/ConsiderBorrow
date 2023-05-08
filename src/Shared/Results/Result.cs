using System.Net;

namespace ConsiderBorrow.Shared.Results;

public interface IResult
{
    bool Succeeded { get; }
    HttpStatusCode StatusCode { get; }
    string? Message { get; }
}

public record Result(bool Succeeded, HttpStatusCode StatusCode, string? Message) : IResult
{
    private static readonly Result successResult = new Result(true, HttpStatusCode.OK, null);
    private static readonly Result failResult = new Result(false, HttpStatusCode.BadRequest, null);

    public static Result Success()
    {
        return successResult;
    }

    public static Result Success(string message)
    {
        return successResult with { Message = message };
    }

    public static Result Fail()
    {
        return failResult;
    }

    public static Result Fail(string message)
    {
        return failResult with { Message = message };
    }

    public static Result CopyOf(Result copy)
    {
        return new Result(copy.Succeeded, copy.StatusCode, copy.Message);
    }
}