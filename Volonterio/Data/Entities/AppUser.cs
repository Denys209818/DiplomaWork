using Microsoft.AspNetCore.Identity;
using Volonterio.Data.Entities.CustomEntities;

namespace Volonterio.Data.Entities
{
    public class AppUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public virtual ICollection<AppGroup> Groups { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }

        public virtual ICollection<AppFriend> Friends { get; set; }
        public virtual ICollection<AppUserGroup> UserGroups { get; set; }
    }
}
