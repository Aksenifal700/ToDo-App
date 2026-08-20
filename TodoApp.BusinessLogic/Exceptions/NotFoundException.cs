using System.Net;

namespace TodoApp.BusinessLogic.Exceptions;

public class NotFoundException : AppException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    
    public NotFoundException(string message) : base(message) {}
    
}