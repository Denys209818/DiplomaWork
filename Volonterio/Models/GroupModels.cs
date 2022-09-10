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
        public string Param { get; set; }
        public int Page { get; set; }
        public int UserId { get; set; }
    }

    public class SubscribeModal
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }

    public class GetById
    {
        public int Id { get; set; }
    }
    public class GetByName
    {
        public string Name { get; set; }
    }

    public class GetByIdResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class GetGroupDataForMain
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SubscribersCount { get; set; }
        public string Image { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
