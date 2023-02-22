using api.Entities;
using api.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Services
{
  public class TokenService : ITokenService
  {
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config)
    {
      _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));
    }

    public string CreateToken(AppUser user)
    {
      // Adding Claims
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.NameId, user.UserName!)
      };

      // Creating Credentials
      var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

      // Describing how tokens gonna look
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(7),
        SigningCredentials = creds
      };

      // Create and Write Token
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}
