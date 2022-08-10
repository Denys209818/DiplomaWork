namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppPostTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public virtual ICollection<AppPostTagEntity> PostTagEntities { get; set; }
    }
}
