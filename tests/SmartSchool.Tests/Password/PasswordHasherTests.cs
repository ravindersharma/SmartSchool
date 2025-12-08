using FluentAssertions;
using SmartSchool.Application.Auth.Interfaces;
using SmartSchool.Infrastructure.Services.Auth;

namespace SmartSchool.Tests.Password
{
    public class PasswordHasherTests
    {
        private readonly IPasswordHasher _hasher;

        public PasswordHasherTests()
        {
            _hasher = new PasswordHasher(); 
        }

        [Fact]
        public void Hash_ShouldReturnDifferentHashes_ForSamePassword()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hash1 = _hasher.Hash(password);
            var hash2 = _hasher.Hash(password);

            // Assert
            hash1.Should().NotBe(hash2); // uses unique salt so hashes differ
        }

        [Fact]
        public void Verify_ShouldReturnTrue_ForCorrectPassword()
        {
            var password = "TestPassword123!";
            var hash = _hasher.Hash(password);

            var result = _hasher.Verify(password, hash);

            result.Should().BeTrue();
        }

        [Fact]
        public void Verify_ShouldReturnFalse_ForIncorrectPassword()
        {
            var password = "Test@123";
            var wrongPassword = "WrongPass123";

            var hash = _hasher.Hash(password);

            var result = _hasher.Verify(wrongPassword, hash);

            result.Should().BeFalse();
        }
    }
}
