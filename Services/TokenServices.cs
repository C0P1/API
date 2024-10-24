namespace API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entitites;
using Microsoft.IdentityModel.Tokens;

public class TokenServices(IConfiguration config) : ITokenServices
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["Tokenkey"] ?? throw new ArgumentException("TokenKey not found");
        if (tokenKey.Length < 64)
        {
            throw new ArgumentException("TokenKey too short");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserName)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokendDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokendDescriptor);

        return tokenHandler.WriteToken(token);
    }
}