namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppGroupChat
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<AppUser> UsersId { get; set; }
        public virtual ICollection<AppMessage> Messages { get; set; }

    }
}
