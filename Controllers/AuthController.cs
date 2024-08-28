using Microsoft.AspNetCore.Mvc;
using BookStore.Services.Interfaces;
using BookStore.Repositories.Interfaces;

namespace BookStore.Controller{

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IRoleRepository _roleRepository;

    public AuthController(IAuthService authService, IRoleRepository roleRepository)
    {
        _authService = authService;
        _roleRepository = roleRepository;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = _authService.Authenticate(model.Email, model.Password);

        if (user == null)
            return Unauthorized(new { message = "Email or password is incorrect" });

        var token = _authService.GenerateToken(user);
        SetTokenCookie(token);

        return Ok(new {message = "user loggedin"});
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = new User(model.Email, _authService.HashPassword(model.Password));
        bool valid = _authService.RegisterUser(user);
        
        if (!valid) {
            return StatusCode(409, new {message = "Email already exists"});
        }
         // Assign roles to the user
        _authService.AssignRoleToUser(user.UserId, model.Role);
    
        return Ok(new {message = "user registered"});
    }

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("AuthToken", token, cookieOptions);
    }
}
}
