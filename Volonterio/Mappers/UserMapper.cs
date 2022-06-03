using AutoMapper;
using Volonterio.Data.Entities;
using Volonterio.Models;

namespace Volonterio.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterUserModel, AppUser>()
                .ForMember(x => x.UserName, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.FirstName, y => y.MapFrom(z => z.FirstName))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(z => z.Phone))
                .ForMember(x => x.SecondName, y => y.MapFrom(z => z.SecondName))
                .ForMember(x => x.Image, y => y.Ignore());
        }
    }
}
