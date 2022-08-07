using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volonterio.Models
{
    [Table("tblFriendChat")]
    public class FriendChatModels
    {
        [Key]
        public int Id { get; set; }
        public int UserOneId { get; set; }
        public int UserTwoId { get; set; }

        //public virtual ICollection<MessageModels> Messages { get; set; }

    }
}
