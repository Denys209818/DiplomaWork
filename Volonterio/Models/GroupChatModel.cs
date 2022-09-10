namespace Volonterio.Models
{
    public class IAddMessage
    {
        public int GroupId { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class GetMessage
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public string FullName { get; set; }
    }
}
