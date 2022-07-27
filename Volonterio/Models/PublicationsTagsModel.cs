using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volonterio.Models
{
    [Table("tblPublicationsTags")]
    public class PublicationsTagsModel
    {
        public int PublicationsId { get; set; }
        public int TegsId { get; set; }

        public virtual PublicationsModels publications { get; set; }
        public virtual TagsModels tags { get; set; }
    }
}
