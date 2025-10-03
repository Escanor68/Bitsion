using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonasABM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Usuarios demo (en producción esto vendría de una base de datos)
        var users = new[]
        {
            new { Username = "admin", Password = "admin123", Role = "Admin" },
            new { Username = "consultor", Password = "consultor123", Role = "Consultor" }
        };

        var user = users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        
        if (user == null)
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        var token = GenerateJwtToken(user.Username, user.Role);
        
        return Ok(new
        {
            token,
            username = user.Username,
            role = user.Role,
            message = "Login exitoso"
        });
    }

    private string GenerateJwtToken(string username, string role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "MiClaveSecretaSuperSeguraParaJWT2024!";
        var issuer = jwtSettings["Issuer"] ?? "PersonasABM";
        var audience = jwtSettings["Audience"] ?? "PersonasABMUsers";
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
