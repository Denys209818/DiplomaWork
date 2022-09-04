namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppLike
    {
        public AppPost Post { get; set; }
        public AppUser User { get; set; }
        public int PostId { get; set; }
        public long UserId { get; set; }
    }
}
