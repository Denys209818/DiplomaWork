namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppUserGroup
    {
        public int GroupId { get; set; }
        public long UserId { get; set; }
        public virtual AppGroup Group { get; set; }
        public virtual AppUser User { get; set; }
    }
}
