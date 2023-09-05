namespace Gym.Dto
{
    public class PasswordResetDto
    {
        public String Email { get; set; }
        public String NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }
    }
}