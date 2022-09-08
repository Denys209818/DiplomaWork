namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppFriendMessage
    {
        public int Id { get; set; }
        public int UserFriendId { get; set; }
        public long UserId { get; set; }
        public AppUserFriend UserFriend { get; set; }
        public AppUser User { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
