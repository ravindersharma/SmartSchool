using FluentAssertions;
using SmartSchool.Infrastructure.Services;

namespace SmartSchool.Tests.Password
{
    public class PasswordHasherTests
    {
        [Fact]
        public void Hash_ShouldReturnDifferentHashs_ForSamePassword()
        {
            // Arrange
            var password = "TestPassword123!";
            // Act
            var hash1 = PasswordHasher.Hash(password);
            var hash2 = PasswordHasher.Hash(password);
            // Assert
            //Assert.NotEqual(hash1, hash2);
            //Using FluentAssertions for better assertion messages
            hash1.Should().NotBe(hash2);
        }


        [Fact]
        public void Verify_ShouldReturnTrue_ForCorrectPassword()
        {
            // Arrange
            var password = "TestPassword123!";
            var hash = PasswordHasher.Hash(password);
            // Act
            var result = PasswordHasher.Verify(password, hash);
            // Assert
            //Assert.True(result);
            result.Should().BeTrue();
        }


        [Fact]
        public void Verify_ShouldReturnFalse_ForIncorrectPassword()
        {
            // Arrange
            var password = "Test@123";
            string wrongPassword = "WrongPass123";
            var hash = PasswordHasher.Hash(password);

            //Act
            var result = PasswordHasher.Verify(wrongPassword, hash);
            //Asert
            //Assert.False(result);
            result.Should().BeFalse();
        }
    }
}
