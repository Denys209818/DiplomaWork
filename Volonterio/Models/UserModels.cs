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

    public class UserFriendReturned
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string ChatId { get; set; }
        public bool IsFriend { get; set; } = false;
    }

    public class FriendMessageInfo
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string FullName { get; set; }
        public long UserId { get; set; }
        public string Image { get; set; }
    }

    public class ChatFriend
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string FullName { get; set; }
        public int ChatId { get; set; }
        public string Image { get; set; }
    }

    public class UserDataProfile
    {
        public int PostsCount { get; set; }
        public int GroupsCount { get; set; }
        public int FriendsCount { get; set; }
    }

    public class ExternalLoginRequest
    {
        public string Provider { get; set; }
        public string Token { get; set; }
    }

    public class ReturnedSearchUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsFriend { get; set; }
    }
}
