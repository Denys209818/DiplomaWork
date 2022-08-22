using AutoMapper;
using Volonterio.Data.Entities;
using Volonterio.Data.Entities.CustomEntities;
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
                .ForMember(x => x.Image, y => y.MapFrom(z => "default.jpg"));

            //CreateMap<EditUserModel, AppUser>()
            //    .ForMember(x  => x.)s
            CreateMap<CreateGroup, AppGroup>()
                .ForMember(x => x.Title, x => x.MapFrom(y => y.Title))
                .ForMember(x => x.Meta, x => x.MapFrom(y => y.Meta))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Description))
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.UserId, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Posts, x => x.Ignore())
                ;

            CreateMap<AppGroup, GroupReturn>()
                .ForMember(x => x.Title, x => x.MapFrom(y => y.Title))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Meta, x => x.MapFrom(y => y.Meta))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Description))
                .ForMember(x => x.Image, x => x.MapFrom(y => y.Image))
                .ForMember(x => x.Tags, x => x.Ignore())
                ;
            CreateMap<AppGroup, GetByIdResult>()
                .ForMember(x => x.Id, y => y.MapFrom(x => x.Id))
                .ForMember(x => x.Title, y => y.MapFrom(x => x.Title));
        }
    }
}
