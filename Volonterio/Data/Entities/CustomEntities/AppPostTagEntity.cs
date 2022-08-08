namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppPostTagEntity
    {
        public virtual AppPost Post { get; set; }
        public int PostId { get; set; }

        public virtual AppPostTag PostTag { get; set; }
        public int PostTagId { get; set; }
    }
}
