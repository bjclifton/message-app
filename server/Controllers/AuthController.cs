using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using server.Models;
using server.Services;


namespace server.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {

        // Check if user with the same username or email already exists, send back appropriate response
        var existingUser = await _userService.GetByUsername(user.Username);
        if (existingUser != null)
        {
            return BadRequest("User with this username already exists");
        }

        existingUser = await _userService.GetByEmail(user.Email);
        if (existingUser != null)
        {
            return BadRequest("User with this email already exists");
        }


        user.PasswordHash = HashPassword(user.Password);
        user.Password = null;
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;

        await _userService.Create(user);

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var existingUser = await _userService.GetByUsername(user.Username);

        if (existingUser == null)
        {
            return BadRequest("Invalid username or password");
        }

        if (!VerifyPassword(user.Password, existingUser.PasswordHash))
        {
            return BadRequest("Invalid username or password");
        }

        var token = GenerateJwtToken(existingUser);

        return Ok(new { token });
    }

    // Helper method for hashing passwords
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Helper method for verifying passwords
    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

}
