namespace Gym.Dto
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }
        public String LastName { get; set; }

        public string Email { get; set; }
        public DateTime DateofBirth { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}