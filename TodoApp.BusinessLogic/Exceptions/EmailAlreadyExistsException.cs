using System.Net;

namespace TodoApp.BusinessLogic.Exceptions;

public class EmailAlreadyExistsException : AlreadyExistsException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    
    public EmailAlreadyExistsException(string message) : base(message) {}
}