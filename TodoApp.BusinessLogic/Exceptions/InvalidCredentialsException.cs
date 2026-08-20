using System.Net;

namespace TodoApp.BusinessLogic.Exceptions;

public class InvalidCredentialsException : AppException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    
    public InvalidCredentialsException(string message) : base(message) {}
    
}