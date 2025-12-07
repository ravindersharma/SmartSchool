using FluentAssertions;
using NSubstitute;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Application.Users.Interfaces;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.Enums;
using SmartSchool.Infrastructure.Services;
using SmartSchool.Shared;

namespace SmartSchool.Tests.Students
{
    public class StudentServiceTests
    {
        private readonly IStudentRepository _studentRepo = Substitute.For<IStudentRepository>();
        private readonly IUserRepository _userRepo = Substitute.For<IUserRepository>();
        private readonly IStudentService _svc;

        public StudentServiceTests()
        {
            _svc = new StudentService(_studentRepo, _userRepo);
        }

        // ----------------------------------------------------
        // CREATE
        // ----------------------------------------------------

        [Fact]
        public async Task CreateAsync_ShouldReturnFail_WhenUserNotFound()
        {
            _userRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((User?)null);

            var dto = new CreateStudentDto(Guid.NewGuid(), "Name", 5, DateTime.UtcNow.AddYears(-10), "IN", "NID123");

            var res = await _svc.CreateAsync(dto, CancellationToken.None);

            res.IsFailed.Should().BeTrue();
            res.Errors.First().Message.Should().Contain("User not found");
        }


        [Fact]
        public async Task CreateAsync_ShouldReturnStudent_WhenUserExists()
        {
            var userId = Guid.NewGuid();

            var dto = new CreateStudentDto(
                userId,
                "Full Name",
                7,
                DateTime.UtcNow.AddYears(-12),
                "IN",
                "N123"
            );

            _userRepo.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                .Returns(new User
                {
                    Id = userId,
                    Email = "a@b.com",
                    UserName = "u",
                    Role = Role.Student
                });

            Student? saved = null;

            _studentRepo.When(x => x.AddAsync(Arg.Any<Student>(), Arg.Any<CancellationToken>()))
                .Do(ci =>
                {
                    saved = ci.ArgAt<Student>(0);
                    saved.Id = Guid.NewGuid(); // simulate EF Core ID creation
                });

            _studentRepo.GetFullByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(ci =>
                {
                    saved!.FullName = dto.FullName;
                    saved.Grade = dto.Grade;
                    saved.DOB = dto.DOB;
                    saved.Nationality = dto.Nationality;
                    saved.NationalId = dto.NationalId;

                    saved.User = new User
                    {
                        Id = userId,
                        Email = "a@b.com",
                        UserName = "u",
                        Role = Role.Student
                    };

                    return saved;
                });

            var res = await _svc.CreateAsync(dto, CancellationToken.None);

            res.IsSuccess.Should().BeTrue();
            res.Value.FullName.Should().Be("Full Name");
            res.Value.Grade.Should().Be(7);
        }

        // ----------------------------------------------------
        // UPDATE
        // ----------------------------------------------------

        [Fact]
        public async Task UpdateAsync_ShouldReturnFail_WhenStudentNotFound()
        {
            _studentRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Student?)null);

            var dto = new UpdateStudentDto("Name", 6, DateTime.UtcNow.AddYears(-11), "IN", "NID124");

            var res = await _svc.UpdateAsync(Guid.NewGuid(), dto, CancellationToken.None);

            res.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedStudent_WhenStudentExists()
        {
            var id = Guid.NewGuid();
            var student = new Student
            {
                Id = id,
                FullName = "Old Name",
                Grade = 5,
                DOB = DateTime.UtcNow.AddYears(-10),
                Nationality = "IN",
                NationalId = "NID123",
                UserId = Guid.NewGuid()
            };

            _studentRepo.GetByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(student);

            var dto = new UpdateStudentDto("New Name", 6, DateTime.UtcNow.AddYears(-11), "IN", "NID124");

            var res = await _svc.UpdateAsync(id, dto, CancellationToken.None);

            res.IsSuccess.Should().BeTrue();
            res.Value.FullName.Should().Be("New Name");
            res.Value.Grade.Should().Be(6);
        }

        // ----------------------------------------------------
        // GET BY ID
        // ----------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFail_WhenStudentNotFound()
        {
            _studentRepo.GetFullByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Student?)null);

            var res = await _svc.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

            res.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStudent_WhenStudentExists()
        {
            var id = Guid.NewGuid();
            var student = new Student { Id = id, FullName = "Existing", User = new User() };

            _studentRepo.GetFullByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns(student);

            var res = await _svc.GetByIdAsync(id, CancellationToken.None);

            res.IsSuccess.Should().BeTrue();
            res.Value.Id.Should().Be(id);
        }

        // ----------------------------------------------------
        // DELETE
        // ----------------------------------------------------

        [Fact]
        public async Task DeleteAsync_ShouldReturnFail_WhenStudentNotFound()
        {
            _studentRepo.GetFullByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Student?)null);

            var res = await _svc.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

            res.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFail_WhenNotSoftDeleted()
        {
            var st = new Student { Id = Guid.NewGuid(), IsDeleted = false };

            _studentRepo.GetFullByIdAsync(st.Id, Arg.Any<CancellationToken>())
                .Returns(st);

            var res = await _svc.DeleteAsync(st.Id, CancellationToken.None);

            res.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldSucceed_WhenSoftDeleted()
        {
            var st = new Student { Id = Guid.NewGuid(), IsDeleted = true };

            _studentRepo.GetFullByIdAsync(st.Id, Arg.Any<CancellationToken>())
                .Returns(st);

            await _svc.DeleteAsync(st.Id, CancellationToken.None);

            await _studentRepo.Received(1).DeleteAsync(st, Arg.Any<CancellationToken>());
        }


        // ----------------------------------------------------
        // GET BY USER ID
        // ----------------------------------------------------

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnFail_WhenNotFound()
        {
            _studentRepo.GetByUserIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Student?)null);

            var res = await _svc.GetByUserIdAsync(Guid.NewGuid(), CancellationToken.None);

            res.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnStudent_WhenExists()
        {
            var uid = Guid.NewGuid();
            var st = new Student { Id = Guid.NewGuid(), UserId = uid };

            _studentRepo.GetByUserIdAsync(uid, Arg.Any<CancellationToken>())
                .Returns(st);

            var res = await _svc.GetByUserIdAsync(uid, CancellationToken.None);

            res.IsSuccess.Should().BeTrue();
            res.Value.UserId.Should().Be(uid);
        }

        // ----------------------------------------------------
        // PAGED
        // ----------------------------------------------------

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedData()
        {
            var list = new List<Student>
            {
                new Student { Id = Guid.NewGuid(), FullName = "A", User = new User() },
                new Student { Id = Guid.NewGuid(), FullName = "B", User = new User() }
            };

            var paged = new PagedResult<Student>(list, 2, 1, 10);

            _studentRepo.GetPagedAsync(1, 10, Arg.Any<CancellationToken>())
                .Returns(paged);

            var res = await _svc.GetPagedAsync(1, 10, CancellationToken.None);

            res.IsSuccess.Should().BeTrue();
            res.Value.Items.Should().HaveCount(2);
            res.Value.TotalCount.Should().Be(2);
        }

        // ----------------------------------------------------
        // SOFT DELETE
        // ----------------------------------------------------

        [Fact]
        public async Task SoftDeleteAsync_ShouldReturnFail_WhenNotFound()
        {
            _studentRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns((Student?)null);

            var res = await _svc.SoftDeleteAsync(Guid.NewGuid(), CancellationToken.None);

            res.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task SoftDeleteAsync_ShouldReturnSuccess_WhenValid()
        {
            var st = new Student { Id = Guid.NewGuid(), IsDeleted = false };

            _studentRepo.GetByIdAsync(st.Id, Arg.Any<CancellationToken>())
                .Returns(st);

            var res = await _svc.SoftDeleteAsync(st.Id, CancellationToken.None);

            res.IsSuccess.Should().BeTrue();
            st.IsDeleted.Should().BeTrue();
        }
    }
}
