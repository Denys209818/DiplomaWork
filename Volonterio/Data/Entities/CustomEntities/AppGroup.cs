namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Meta { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public virtual AppUser User { get; set; }
        public string Image { get; set; }
        public virtual ICollection<AppGroupTag> AppGroupTags { get; set; }
        public virtual ICollection<AppPost> Posts { get; set; }
        public virtual ICollection<AppUserGroup> UserGroups { get; set; }
    }
}
