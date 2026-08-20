using System.Net;

namespace TodoApp.BusinessLogic.Exceptions;

public abstract class AppException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }
    protected AppException(string message) : base(message) {}
}