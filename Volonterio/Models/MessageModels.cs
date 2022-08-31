using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volonterio.Models
{
    [Table("tblMessage")]
    public class MessageModels
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int ChadId { get; set; }
        public DateTime DateCreated { get; set; }
        public long DateCreatedUnix { get; set; }
        
    }
}
