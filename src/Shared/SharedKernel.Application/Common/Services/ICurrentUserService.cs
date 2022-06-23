using System.Security.Claims;

namespace SharedKernel.Application.Common.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    ClaimsPrincipal? User { get; }
}