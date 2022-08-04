namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppPostTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public int PostId { get; set; }
        public virtual AppPost Post { get; set; }
    }
}
