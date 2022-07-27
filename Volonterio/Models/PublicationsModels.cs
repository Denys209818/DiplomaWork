using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volonterio.Models
{
    [Table("tblPublications")]
    public class PublicationsModels
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string TegsSearch { get; set; }
        [Required]
        public int GroupsId { get; set; }
        [Required]
        public string Image { get; set; }
        public virtual ICollection<PublicationsTagsModel> idPublications { get; set; }
        public virtual GroupsModels idgroups { get; set; }
    }
}
