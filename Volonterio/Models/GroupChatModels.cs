using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volonterio.Data.Entities;

namespace Volonterio.Models
{
    [Table("tblGroupChat")]
    public class GroupChatModels
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        //public virtual ICollection<AppUser> UserId { get; set; }
        //public virtual ICollection<MessageModels> Messages { get; set; }

    }
}
