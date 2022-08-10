namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public DateTime DateCreated { get; set; }
        public long DateCreatedUnix { get; set; }
    }
}
