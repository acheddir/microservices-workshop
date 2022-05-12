using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SharedKernel.Application.Common.Services;
using SharedKernel.Extensions;

namespace OrderMgmt.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.GetUserId();
    public string? UserName => _httpContextAccessor.HttpContext?.User?.GetUserName();
    
    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
}