using Microsoft.AspNetCore.Identity;

namespace Gym.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }
        public String LastName { get; set; }
        public DateTime DateofBirth { get; set; }
        public String? ProfilePicture { get; set; }
    }
}