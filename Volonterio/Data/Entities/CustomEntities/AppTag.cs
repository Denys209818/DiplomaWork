namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public virtual ICollection<AppGroupTag> AppGroupTags { get; set; }
    }
}
