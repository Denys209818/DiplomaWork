namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual ICollection<AppPostImage> Images { get; set; }
        public virtual ICollection<AppPostTag> Tags { get; set; }


        public int GroupId { get; set; }
        public virtual AppGroup Group { get; set; }
    }
}
