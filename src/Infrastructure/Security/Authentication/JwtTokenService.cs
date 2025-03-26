using Application.Common.Security.Authentication;
using Application.Common.Security.Identity;

using Infrastructure.Security.Authentication.Settings;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Security.Authentication;

internal sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(IdentityUser user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256
         );

        List<Claim> claims = [
             new Claim(JwtRegisteredClaimNames.Sub, user.Id),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        claims.AddRange(
            from role in user.Roles
            select new Claim(ClaimTypes.Role, role.Name)
        );

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
