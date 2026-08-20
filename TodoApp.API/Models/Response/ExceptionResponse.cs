using System.Net;

namespace TodoApp.API.Models.Response;

public record ExceptionResponse (HttpStatusCode StatusCode, string Message);
