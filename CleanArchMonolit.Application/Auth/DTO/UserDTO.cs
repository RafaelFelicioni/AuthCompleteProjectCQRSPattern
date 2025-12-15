using CleanArchMonolit.Domain.Auth.Entities;

namespace CleanArchMonolit.Application.Auth.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProfileName { get; set; }
        public int ProfileId { get; set; }

        public static implicit operator UserDTO(User entity)
        {
            if (entity == null) return null;
            return new UserDTO
            {
                Id = entity.Id,
                UserName = entity.Username,
                ProfileId = entity.ProfileId,
                ProfileName = entity.Profile.ProfileName,
            };
        }
    }
}
