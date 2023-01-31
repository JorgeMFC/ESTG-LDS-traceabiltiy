using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyTrace.Utils
{
    public class JwtAuthenticationManager
    {
        //key declaration
        private readonly string _JWTScretKey;

        public JwtAuthenticationManager(IConfiguration configuration)
        {
            _JWTScretKey = configuration["AppSettings:JWT:ScretKey"];
        }

        public string? CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Authentication, user.WalletAddress)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_JWTScretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string? GetUserWallet(string? token)
        {
            if (token != null)
            {
                token = token.Replace($"{JwtBearerDefaults.AuthenticationScheme} ", "");
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var wallet = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Authentication).Value;
                return wallet;
            }
            return null;
        }
    }
}
