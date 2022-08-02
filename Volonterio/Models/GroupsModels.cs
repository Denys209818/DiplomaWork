using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volonterio.Models
{

    [Table("tblGroups")]
    public class GroupsModels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Target { get; set; }

        public virtual ICollection<PublicationsModels> IdgroupsPublications { get; set; }
        public virtual ICollection<UsersModels> IdGroupsUsers { get; set; }
    }
}
