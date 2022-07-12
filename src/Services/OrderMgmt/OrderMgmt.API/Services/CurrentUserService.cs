using System.Security.Claims;
using SharedKernel.Extensions;

namespace OrderMgmt.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.GetUserId();
    public string? UserName => _httpContextAccessor.HttpContext?.User?.GetUserName();
    public string? Email => _httpContextAccessor.HttpContext?.User?.GetUserEmail();
    
    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
}
