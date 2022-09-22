namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppUserFriend
    {
        public int Id { get; set; }
        public bool IsFriend { get; set; }
        public virtual ICollection<AppFriend> AppFriends { get; set; }
        public virtual ICollection<AppFriendMessage> FriendMessages { get; set; }
    }
}
