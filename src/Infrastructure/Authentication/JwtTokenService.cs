using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;

/// <summary>
/// Implementation of the token generator service.
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    /// <summary>
    /// Settings containing token information.
    /// </summary>
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
    /// </summary>
    /// <param name="jwtOptions">The jwt settings options.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public JwtTokenService(IOptions<JwtSettings> jwtOptions, IUnitOfWork unitOfWork)
    {
        _jwtSettings = jwtOptions.Value;
    }

    /// <inheritdoc/>
    public string GenerateToken(User user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256
         );

        List<Claim> claims = [
             new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString(CultureInfo.InvariantCulture)),
             new Claim(JwtRegisteredClaimNames.Name, user.Name),
             new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];


        claims.AddRange(
            from roleName in user.GetRoleNames()
            select new Claim(ClaimTypes.Role, roleName)
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
