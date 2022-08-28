namespace Volonterio.Models
{
    public class CreatePublicationModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
        public int GroupId { get; set; }
        public List<CreatePublicationImageModel> Images { get; set; }

    }

    public class CreatePublicationImageModel
    {
        public string Image { get; set; }
    }

    public class PublicationImageModel
    {
        public string Image { get; set; }
    }

    public class DeletePublicationModel
    {
        public int PostId { get; set; }
    }

    public class EditPublicationModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
        public List<PublicationImageModel> Images { get; set; }
        public int PublicationId { get; set; }
    }

    public class DeleteImagePublicationModel
    {
        public string Image { get; set; }
    }

    public class GetPostByGroupId
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
    }
}
