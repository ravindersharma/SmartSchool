namespace SmartSchool.Application.Users.Dtos
{
    public record UserPagedRequest(int page = 1, int pageSize = 10);
}
