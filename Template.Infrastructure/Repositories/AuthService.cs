using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Template.Application.DTOs;
using Template.Application.Services;
using Template.Domain.Entities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Repositories
{
    /// <summary>
    /// Provides authentication and authorization services, including user registration, login, and token management.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly TemplateDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly PasswordHasher<User> _passwordHasher = new();

        private const int PH_TIME_OFFSET_HOURS = 8;
        private const int REFRESH_TOKEN_EXPIRY_DAYS = 7;
        private const int ACCESS_TOKEN_EXPIRY_DAYS = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing user data.</param>
        /// <param name="configuration">Configuration for reading app settings.</param>
        /// <param name="mapper">Object mapper for DTOs.</param>
        /// <param name="logger">Logger for recording authentication events.</param>
        public AuthService(TemplateDbContext context, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and generates an access token if credentials are valid.
        /// </summary>
        /// <param name="request">User login request containing username and password.</param>
        /// <returns>A <see cref="TokenResponseDto"/> if authentication succeeds, otherwise null.</returns>
        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User '{Username}' not found.", request.Username);
                return null;
            }

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login failed: Invalid password for user '{Username}'.", request.Username);
                return null;
            }

            _logger.LogInformation("User '{Username}' logged in successfully.", request.Username);
            return await CreateTokenResponseAsync(user);
        }

        /// <summary>
        /// Registers a new user if the username is not already taken.
        /// </summary>
        /// <param name="request">User registration request containing username and password.</param>
        /// <returns>The created <see cref="User"/> object, or null if the username is already taken.</returns>
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                _logger.LogWarning("Registration failed: Username '{Username}' already exists.", request.Username);
                return null;
            }

            var user = new User
            {
                Username = request.Username,
                PasswordHash = _passwordHasher.HashPassword(null!, request.Password) // Hash password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User '{Username}' registered successfully.", request.Username);
            return user;
        }

        /// <summary>
        /// Refreshes a user's access token using a valid refresh token.
        /// </summary>
        /// <param name="request">The refresh token request containing user ID and refresh token.</param>
        /// <returns>A new <see cref="TokenResponseDto"/> if the refresh token is valid, otherwise null.</returns>
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user == null)
            {
                _logger.LogWarning("Refresh token validation failed for user ID '{UserId}'.", request.UserId);
                return null;
            }

            _logger.LogInformation("Refresh token validated successfully for user ID '{UserId}'.", request.UserId);
            return await CreateTokenResponseAsync(user);
        }

        /// <summary>
        /// Validates a refresh token for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user requesting a refresh.</param>
        /// <param name="refreshToken">The refresh token to validate.</param>
        /// <returns>The user if validation succeeds, otherwise null.</returns>
        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Creates a token response containing an access token and refresh token.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>A <see cref="TokenResponseDto"/> containing tokens.</returns>
        private async Task<TokenResponseDto> CreateTokenResponseAsync(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        /// <summary>
        /// Generates and saves a new refresh token for a user.
        /// </summary>
        /// <param name="user">The user for whom to generate a refresh token.</param>
        /// <returns>The newly generated refresh token.</returns>
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(PH_TIME_OFFSET_HOURS).AddDays(REFRESH_TOKEN_EXPIRY_DAYS);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        /// <summary>
        /// Generates a cryptographically secure random refresh token.
        /// </summary>
        /// <returns>A base64-encoded random string.</returns>
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Generates a JWT access token for a user.
        /// </summary>
        /// <param name="user">The user for whom to generate the access token.</param>
        /// <returns>The generated JWT token as a string.</returns>
        private string GenerateAccessToken(User user)
        {
            var roleName = _context.Roles
                            .Where(r => r.Id == user.RoleId)
                            .Select(r => r.RoleName)
                            .FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetValue<string>("AppSettings:Token") ?? throw new InvalidOperationException("JWT Token key is missing")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(ACCESS_TOKEN_EXPIRY_DAYS),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
