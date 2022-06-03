namespace Volonterio.Models
{
    public class RegisterUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public IFormFile Image { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
