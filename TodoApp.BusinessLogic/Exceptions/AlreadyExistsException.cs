using System.Net;

namespace TodoApp.BusinessLogic.Exceptions;

public class AlreadyExistsException : AppException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public AlreadyExistsException(string message) : base(message) {}
}