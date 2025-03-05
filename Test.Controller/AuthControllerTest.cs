using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Template.Application.DTOs;
using Template.Application.Services;
using Template.Domain.Entities;
using Template.Web.API.Controllers;

namespace Test.Controller
{
    public class AuthControllerTest
    {
        private readonly IAuthService _authService;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            // Mocking dependencies
            _authService = A.Fake<IAuthService>();

            // SUT (System Under Test)
            _authController = new AuthController(_authService);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsRegistered()
        {
            // Arrange
            var registerDto = new RegisterDto { Username = "testuser", Password = "Password123!" };
            var user = new User { Username = "testuser", RoleId = Guid.NewGuid() };

            A.CallTo(() => _authService.RegisterAsync(registerDto)).Returns(user);

            // Act
            var result = await _authController.Register(registerDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUsernameAlreadyExists()
        {
            // Arrange
            var registerDto = new RegisterDto { Username = "existinguser", Password = "Password123!" };

            A.CallTo(() => _authService.RegisterAsync(registerDto)).Returns(Task.FromResult<User?>(null));

            // Act
            var result = await _authController.Register(registerDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Value.Should().Be("Username already exists.");
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenValidCredentials()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "Password123!" };
            var tokenResponse = new TokenResponseDto { AccessToken = "valid_token", RefreshToken = "refresh_token" };

            A.CallTo(() => _authService.LoginAsync(userDto)).Returns(tokenResponse);

            // Act
            var result = await _authController.Login(userDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(tokenResponse);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenInvalidCredentials()
        {
            // Arrange
            var userDto = new UserDto { Username = "wronguser", Password = "wrongpassword" };

            A.CallTo(() => _authService.LoginAsync(userDto)).Returns(Task.FromResult<TokenResponseDto?>(null));

            // Act
            var result = await _authController.Login(userDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Value.Should().Be("Invalid username or password.");
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnOk_WhenValidToken()
        {
            // Arrange
            var refreshRequest = new RefreshTokenRequestDto { RefreshToken = "valid_refresh_token" };
            var tokenResponse = new TokenResponseDto { AccessToken = "new_access_token", RefreshToken = "new_refresh_token" };

            A.CallTo(() => _authService.RefreshTokensAsync(refreshRequest)).Returns(tokenResponse);

            // Act
            var result = await _authController.RefreshToken(refreshRequest);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(tokenResponse);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnUnauthorized_WhenInvalidToken()
        {
            // Arrange
            var refreshRequest = new RefreshTokenRequestDto { RefreshToken = "invalid_refresh_token" };

            A.CallTo(() => _authService.RefreshTokensAsync(refreshRequest)).Returns(Task.FromResult<TokenResponseDto?>(null));

            // Act
            var result = await _authController.RefreshToken(refreshRequest);

            // Assert
            result.Result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            unauthorizedResult!.Value.Should().Be("Invalid refresh token.");
        }
    }
}
