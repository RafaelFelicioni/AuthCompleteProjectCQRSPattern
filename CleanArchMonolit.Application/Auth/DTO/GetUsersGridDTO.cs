using CleanArchMonolit.Shared.DTO.Grid;

namespace CleanArchMonolit.Application.Auth.DTO
{
    public class GetUsersGridDTO : GridQueryDTO
    {
        public int? UserId { get; set; }
        public int? ProfileId { get; set; }
    }
}
