namespace PCF.API.Dto
{
    public class LoginResponseDto
    {
        public string Name { get; set; } = string.Empty; //Somente para registro.
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmedPassword { get; set; } = string.Empty;
    }
}
