﻿using System.Net;

namespace ConsiderBorrow.Shared.Results;

public interface IResult<out T> : IResult
{
    T? Data { get; }
}

public sealed record Result<T>(bool Succeeded, HttpStatusCode StatusCode, string StatusCodeDescription, IEnumerable<string> Messages, T? Data)
    : Result(Succeeded, StatusCode, StatusCodeDescription, Messages), IResult<T>
{
    private static readonly Result<T> successResult = new Result<T>(true, HttpStatusCode.OK, StatusCodeDescriptions.None, Array.Empty<string>(), default);
    private static readonly Result<T> failResult = new Result<T>(false, HttpStatusCode.BadRequest, StatusCodeDescriptions.None, Array.Empty<string>(), default);

    public static new Result<T> Success()
    {
        return successResult;
    }

    public static Result<T> Success(T data)
    {
        return successResult with { Data = data };
    }

    public static new Result<T> Success(string message)
    {
        return successResult with { Messages = new string[] { message } };
    }

    public static Result<T> Success(T data, string message)
    {
        return successResult with { Data = data, Messages = new string[] { message } };
    }

    public static Result<T> Success(T data, IEnumerable<string> messages)
    {
        return successResult with { Data = data, Messages = messages };
    }

    public static new Result<T> Fail()
    {
        return failResult;
    }

    public static Result<T> Fail(T data)
    {
        return failResult with { Data = data };
    }

    public static Result<T> Fail(T data, string message)
    {
        return failResult with { Data = data, Messages = new string[] { message } };
    }

    public static new Result<T> Fail(string message)
    {
        return failResult with { Messages = new string[] { message } };
    }

    public static new Result<T> Fail(IEnumerable<string> messages)
    {
        return failResult with { Messages = messages };
    }

    public static Result<T> CopyOf(Result copy, T? data = default)
    {
        return new Result<T>(copy.Succeeded, copy.StatusCode, copy.StatusCodeDescription, copy.Messages, data);
    }
}