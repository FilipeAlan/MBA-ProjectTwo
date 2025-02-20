namespace PCF.Core.Dtos.Login
{
    public class RegisterResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? ConfirmedPassword { get; set; }
    }
}
