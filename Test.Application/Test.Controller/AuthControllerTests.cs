using Microsoft.AspNetCore.Mvc;
using Moq;
using Template.Application.DTOs;
using Template.Application.Services;
using Template.Domain.Entities;
using Template.Web.API.Controllers;
using Xunit;
using System;
using System.Threading.Tasks;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    // ----- Register Tests -----
    [Fact]
    public async Task Register_ShouldReturnOk_WhenUserIsCreated()
    {
        // Arrange
        var request = new UserDto { Username = "OBPI-1870", Password = "password123" };
        var user = new User { Id = Guid.NewGuid(), Username = request.Username };

        _mockAuthService.Setup(s => s.RegisterAsync(It.IsAny<UserDto>()))
                        .ReturnsAsync(user);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(request.Username, returnedUser.Username);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenUserAlreadyExists()
    {
        // Arrange
        var request = new UserDto { Username = "existingUser", Password = "password123" };

        _mockAuthService.Setup(s => s.RegisterAsync(It.IsAny<UserDto>()))
                        .ReturnsAsync((User)null!); // Simulate existing user

        // Act
        var result = await _controller.Register(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Username already exists.", badRequestResult.Value);
    }

    // ----- Login Tests -----
    [Fact]
    public async Task Login_ShouldReturnOk_WhenValidCredentialsProvided()
    {
        // Arrange
        var request = new UserDto { Username = "testuser", Password = "password123" };
        var tokenResponse = new TokenResponseDto { AccessToken = "access_token", RefreshToken = "refresh_token" };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<UserDto>()))
                        .ReturnsAsync(tokenResponse);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTokens = Assert.IsType<TokenResponseDto>(okResult.Value);
        Assert.Equal("access_token", returnedTokens.AccessToken);
        Assert.Equal("refresh_token", returnedTokens.RefreshToken);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenInvalidCredentialsProvided()
    {
        // Arrange
        var request = new UserDto { Username = "testuser", Password = "wrongpassword" };

        _mockAuthService.Setup(s => s.LoginAsync(It.IsAny<UserDto>()))
                        .ReturnsAsync((TokenResponseDto)null!); // Invalid credentials

        // Act
        var result = await _controller.Login(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid username or password.", badRequestResult.Value);
    }

    // ----- Refresh Token Tests -----
    [Fact]
    public async Task RefreshToken_ShouldReturnOk_WhenValidTokenProvided()
    {
        // Arrange
        var request = new RefreshTokenRequestDto { RefreshToken = "valid_refresh_token" };
        var tokenResponse = new TokenResponseDto { AccessToken = "new_access_token", RefreshToken = "new_refresh_token" };

        _mockAuthService.Setup(s => s.RefreshTokensAsync(It.IsAny<RefreshTokenRequestDto>()))
                        .ReturnsAsync(tokenResponse);

        // Act
        var result = await _controller.RefreshToken(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTokens = Assert.IsType<TokenResponseDto>(okResult.Value);
        Assert.Equal("new_access_token", returnedTokens.AccessToken);
        Assert.Equal("new_refresh_token", returnedTokens.RefreshToken);
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnUnauthorized_WhenInvalidTokenProvided()
    {
        // Arrange
        var request = new RefreshTokenRequestDto { RefreshToken = "invalid_refresh_token" };

        _mockAuthService.Setup(s => s.RefreshTokensAsync(It.IsAny<RefreshTokenRequestDto>()))
                        .ReturnsAsync((TokenResponseDto)null!); // Simulating invalid token

        // Act
        var result = await _controller.RefreshToken(request);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        Assert.Equal("Invalid refresh token.", unauthorizedResult.Value);
    }
}
