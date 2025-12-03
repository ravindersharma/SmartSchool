using FluentAssertions;
using FluentResults;
using NSubstitute;
using SmartSchool.Application.Auth.Commands.Register;
using SmartSchool.Application.Auth.Commands.RegisterUser;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Application.Auth.Interfaces;

namespace SmartSchool.Tests.Auth
{
    public class RegistrationHandlerTests
    {
        [Fact]
        public async Task Register_ShouldReturnSuccess_WhenValid()
        {
            // Arrange
            var authService = Substitute.For<IAuthService>();
            var handler = new RegisterHandler(authService);
            var dto = new RegisterRequestDto("test@email.com", "test", "Pass@123");
            var expected = new AuthResponseDto(Guid.NewGuid(), dto.Email, dto.UserName, "User", "jwt", "rt", DateTime.UtcNow);

            authService.RegisterAsync(dto, Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Result.Ok(expected));

            // Act
            var result = await handler.Handle(new RegisterCommand(dto, "localhost"), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Email.Should().Be(dto.Email);
        }

        [Fact]
        public async Task Register_ShouldReturnFailure_WhenServiceFail()
        {
            // Arrange
            var authService = Substitute.For<IAuthService>();
            var handler = new RegisterHandler(authService);
            var dto = new RegisterRequestDto("test@email.com", "test", "Pass@123");

            authService.RegisterAsync(dto, Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Result.Fail<AuthResponseDto>("Registration failed"));

            // Act
            var result = await handler.Handle(new RegisterCommand(dto, "localhost"), CancellationToken.None);
            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Register_ShouldReturnFailure_WithEmailExists()
        {
            // Arrange
            var authService = Substitute.For<IAuthService>();
            var handler = new RegisterHandler(authService);
            var dto = new RegisterRequestDto("test@email.com", "test", "Pass@123");
            authService.RegisterAsync(dto, Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Result.Fail<AuthResponseDto>("Email already exists"));

            // Act
            var result = await handler.Handle(new RegisterCommand(dto, "localhost"), CancellationToken.None);

            result.IsFailed.Should().BeTrue();

        }
    }
}
