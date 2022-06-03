using Microsoft.AspNetCore.Identity;

namespace Volonterio.Data.Entities
{
    public class AppUser : IdentityUser<long>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Image { get; set; }
    }
}
