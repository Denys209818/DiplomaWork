using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volonterio.Models
{
    [Table("tblTags")]
    public class TagsModels
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string NameTags { get; set; }

        public virtual ICollection<PublicationsTagsModel> idTags { get; set; }
    }
}
