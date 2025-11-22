using System.Security.Claims;
using MyWebApi.Models;

namespace MyWebApi.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user); // БЕЗ Models.
        string GenerateRefreshToken();
        ClaimsPrincipal GetClaimsFromToken(string token);
    }
}