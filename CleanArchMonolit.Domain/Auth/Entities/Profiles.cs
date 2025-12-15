using System.ComponentModel.DataAnnotations;

namespace CleanArchMonolit.Domain.Auth.Entities
{
    public class Profiles
    {
        public Profiles()
        {
            
        }

        public Profiles(int id, string profileName)
        {
            Id = id;
            ProfileName = profileName;
        }

        public int Id { get; private set; }
        [MaxLength(150)]
        public string ProfileName { get; private set; }
        public void ChangeProfileName(string profileName) => ProfileName = profileName; 
    }
}
