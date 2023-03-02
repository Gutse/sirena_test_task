using System;

namespace RouteSearch.Core.Errors;

public class LogicException: Exception
{
    public Error? Error { get; set; }

    public LogicException(Error error, string message): base(message)
    {
        Error = error;
    }

    public LogicException(Error error)
    {
        Error = error;
    }

    public LogicException(string message): base(message)
    {
    }

    public LogicException(Error error, string? message, Exception? innerException) : base(message, innerException)
    {
        Error = error;
    }
}