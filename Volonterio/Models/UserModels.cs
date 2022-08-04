namespace Volonterio.Models
{
    public class RegisterUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class EditUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string PhoneNumber { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RemoveUserModel
    {
        public string Email { get; set; }
    }

    public class ChangeImageUserModel
    {
        public string Email { get; set; }
        public string ImageBase64 { get; set; }
    }
}
