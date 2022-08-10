namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppGroupTag
    {
        public int GroupId { get; set; }
        public virtual AppGroup Group { get; set; }
        public int TagId { get; set; }
        public virtual AppTag Tag { get; set; }
    }
}
