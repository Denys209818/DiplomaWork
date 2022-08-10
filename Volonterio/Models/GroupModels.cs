namespace Volonterio.Models
{
    public class CreateGroup
    {
        public string Title { get; set; }
        public string Meta { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int UserId { get; set; }
    }

    public class GroupReturn
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Meta { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class DeleteGroup
    {
        public int GroupId { get; set; }
    }

    public class EditGroup
    {
        public int GroupId { get; set; }
        public string Title { get; set; }
        public string Meta { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string ImageBase64 { get; set; }
    }

    public class SearchGroup
    {
        public string param { get; set; }
    }
}
