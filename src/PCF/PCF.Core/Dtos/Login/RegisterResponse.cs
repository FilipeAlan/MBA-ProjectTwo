namespace PCF.Core.Dtos.Login
{
    public class RegisterResponse
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string? ConfirmedPassword { get; set; }
    }
}
