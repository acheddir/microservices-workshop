using System.Security.Claims;

namespace SharedKernel.Infrastructure.Common;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    ClaimsPrincipal? User { get; }
}