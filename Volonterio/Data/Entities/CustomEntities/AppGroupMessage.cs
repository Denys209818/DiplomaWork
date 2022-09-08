namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppGroupMessage
    {
        public int Id { get; set; }
        public virtual AppUser User { get; set; }
        public virtual AppGroup Group { get; set; }
        public long UserId { get; set; }
        public int GroupId { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
