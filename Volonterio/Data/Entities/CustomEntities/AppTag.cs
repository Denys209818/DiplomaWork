namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public int GroupId { get; set; }
        public virtual AppGroup Group { get; set; }
    }
}
