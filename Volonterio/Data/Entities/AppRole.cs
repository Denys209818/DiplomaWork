using Microsoft.AspNetCore.Identity;

namespace Volonterio.Data.Entities
{
    public class AppRole : IdentityRole<long>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
