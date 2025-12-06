using FluentAssertions;
using NSubstitute;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Infrastructure.Services.Auth;

namespace SmartSchool.Tests.Password
{
    public class PasswordHasherTests
    {
        [Fact]
        public void Hash_ShouldReturnDifferentHashs_ForSamePassword()
        {

            var _hasher = Substitute.For<IPasswordHasher>();
            // Arrange
            var password = "TestPassword123!";
            // Act
            var hash1 = _hasher.Hash(password);
            var hash2 = _hasher.Hash(password);
            // Assert
            //Assert.NotEqual(hash1, hash2);
            //Using FluentAssertions for better assertion messages
            hash1.Should().NotBe(hash2);
        }


        [Fact]
        public void Verify_ShouldReturnTrue_ForCorrectPassword()
        {
            var _hasher = Substitute.For<IPasswordHasher>();
            // Arrange
            var password = "TestPassword123!";
            var hash = _hasher.Hash(password);
            // Act
            var result = _hasher.Verify(password, hash);
            // Assert
            //Assert.True(result);
            result.Should().BeTrue();
        }


        [Fact]
        public void Verify_ShouldReturnFalse_ForIncorrectPassword()
        {
            var _hasher = Substitute.For<IPasswordHasher>();
            // Arrange
            var password = "Test@123";
            string wrongPassword = "WrongPass123";
            var hash = _hasher.Hash(password);

            //Act
            var result = _hasher.Verify(wrongPassword, hash);
            //Asert
            //Assert.False(result);
            result.Should().BeFalse();
        }
    }
}
