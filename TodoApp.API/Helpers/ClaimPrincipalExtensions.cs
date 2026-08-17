using System.Security.Claims;

namespace TodoApp.API.Helpers;

public static class ClaimPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst("userid")?.Value;
        
        if(userIdClaim is null || !Guid.TryParse(userIdClaim, out Guid userId))
            throw new UnauthorizedAccessException();
        
        return userId;
    }
}