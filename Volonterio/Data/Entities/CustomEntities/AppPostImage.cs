namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppPostImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int PostId { get; set; }
        public virtual AppPost Post { get; set; }
    }
}
