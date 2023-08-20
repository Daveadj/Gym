namespace Gym.Dto
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }
        public String LastName { get; set; }

        public int? YearOfBirth { get; set; }
        public int? DayOfBirth { get; set; }
    }
}