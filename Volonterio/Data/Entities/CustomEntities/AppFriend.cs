namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppFriend
    {
        public int Id { get; set; }
        public int UserFriendId { get; set; }
        public virtual AppUserFriend UserFriend { get; set; }
        public long UserId { get; set; }
        public virtual AppUser User { get; set; }
        public bool IsFriend { get; set; }
    }
}
