using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookStore.Services.Interfaces;
using BookStore.Repositories.Interfaces;


namespace BookStore.Services{
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _configuration = configuration;
    }

    public User Authenticate(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);
        // if (user == null || user.Password != password)
        // {
        //     return null;
        // }
        bool isVerified = this.VerifyHashedPassword(user.Password, password);
        if (!isVerified)
        {
            return null;
        }

        return user;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Secrets").GetValue<string>("ApiKey") ?? string.Empty);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, string.Join(",", user.Roles.Select(r => r.Name)))
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool RegisterUser(User user)
    {
        // Check if the user already exists
        if (_userRepository.UserExists(user.Email))
        {
            // Handle the case where the user already exists
            return false;
        }
        _userRepository.AddUser(user);
        return true;
    }


        public void AssignRoleToUser(Guid userId, string roleName)
    {
        var role = _roleRepository.GetRoleByName(roleName);
        var user = _userRepository.GetUserById(userId); // Ensure this retrieves the user by ID

        if (user != null && role != null)
        {
            if (!user.Roles.Contains(role)) // Prevent duplicate roles
            {
                user.Roles.Add(role);
                _userRepository.UpdateUser(user); // Update the existing user
            }
        }
    }

    public string HashPassword(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }

        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
        if (providedPassword == null) throw new ArgumentNullException(nameof(providedPassword));

        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
}