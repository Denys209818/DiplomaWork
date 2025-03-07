﻿namespace Volonterio.Data.Entities.CustomEntities
{
    public class AppPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual ICollection<AppPostImage> Images { get; set; }
        public virtual ICollection<AppPostTagEntity> PostTagEntities { get; set; }

        public virtual ICollection<AppLike> Likes { get; set; }

        public int GroupId { get; set; }
        public virtual AppGroup Group { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
