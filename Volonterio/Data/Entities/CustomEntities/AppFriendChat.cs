namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppFriendChat
    {
        public int Id { get; set; }
        public int UserOneId { get; set; }
        public int UserTwoId { get; set; }
        public virtual ICollection<AppMessage> Messages { get; set; }

    }
}
