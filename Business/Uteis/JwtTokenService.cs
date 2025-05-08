using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Domain.Interfaces;
using Domain.Uteis;

namespace Business.Uteis
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SymmetricSecurityKey _signingKey;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            if (string.IsNullOrEmpty(_jwtSettings.SecretKey)) throw new ArgumentNullException(nameof(_jwtSettings.SecretKey), "A SecretKey JWT deve ser configurada.");
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        }

        public string GerarTokenJWT(CWClienteUsuario usuario)
        {
            var claims = GetUserClaims(usuario);
            var tokenOptions = GenerateTokenOptions(claims);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenOptions);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidarTokenJWT(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = _signingKey,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = _signingKey,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }

        private List<Claim> GetUserClaims(CWClienteUsuario usuario)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.sCdUsuario),
                new Claim("TenantId", usuario.nCdCliente.ToString()),
                new Claim("TenantBase", usuario.Cliente?.sNmCliente?.ToString() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Version, "1.0")
            };
        }

        private SecurityTokenDescriptor GenerateTokenOptions(IEnumerable<Claim> claims)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                NotBefore = DateTime.UtcNow
            };
        }
    }
}