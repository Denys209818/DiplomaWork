namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppUserFriend
    {
        public int Id { get; set; }
        public virtual ICollection<AppFriend> AppFriends { get; set; }
    }
}
