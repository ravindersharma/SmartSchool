using FluentAssertions;
using FluentResults;
using NSubstitute;
using SmartSchool.Application.Auth.Commands.Login;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Application.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Tests.Auth
{
    public class LoginHandlerTests
    {
        [Fact]
        public async Task Login_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var authService = Substitute.For<IAuthService>();
            var handler = new LoginHandler(authService);
            var userResponse = new AuthResponseDto(Guid.NewGuid(), "test@email.com", "test", "User", "jwt", "refresh", DateTime.UtcNow);

            authService.LoginAsync(Arg.Any<LoginRequestDto>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Result.Ok(userResponse));

            // Act
            var result = await handler.Handle(new LoginCommand(new LoginRequestDto("test@email.com", "Pass@123"), "127.0.0.1"), CancellationToken.None);


            result.IsSuccess.Should().BeTrue();
            result.Value.Email.Should().Be("test@email.com");
        }

        [Fact]
        public async Task Login_ShouldFail_WhenServiceReturnsFailure()
        {
            // Arrange
            var authService = Substitute.For<IAuthService>();
            var handler = new LoginHandler(authService);
            authService.LoginAsync(Arg.Any<LoginRequestDto>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Result.Fail<AuthResponseDto>("Invalid credentials"));
            // Act
            var result = await handler.Handle(new LoginCommand(new LoginRequestDto("bad@email.com", "WrongPass"), "127.0.0.1"), CancellationToken.None);

            result.IsFailed.Should().BeTrue();
        }
    }
}
