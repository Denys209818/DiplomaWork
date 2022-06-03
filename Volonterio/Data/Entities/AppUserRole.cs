using Microsoft.AspNetCore.Identity;

namespace Volonterio.Data.Entities
{
    public class AppUserRole : IdentityUserRole<long>
    {
        public virtual AppRole Role { get; set; }
        public virtual AppUser User { get; set; }
    }
}
