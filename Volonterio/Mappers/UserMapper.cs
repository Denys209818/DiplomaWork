using AutoMapper;
using Volonterio.Data.Entities;
using Volonterio.Data.Entities.CustomEntities;
using Volonterio.Models;
using Microsoft.EntityFrameworkCore;

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
                .ForMember(x => x.Title, y => y.MapFrom(x => x.Title))
                .ForMember(x => x.Description, y => y.MapFrom(x => x.Description))
                .ForMember(x => x.Image, y => y.MapFrom(z => z.Image));

            CreateMap<AppPost, GetPostByGroupId>()
                .ForMember(x => x.Id, y => y.MapFrom(x => x.Id))
                .ForMember(x=> x.Title, y => y.MapFrom(x => x.Title))
                .ForMember(x=> x.Description, y => y.MapFrom(x => x.Text))
                .ForMember(x=> x.Images, y => y.MapFrom(x => x.Images.Select(x => x.Image)))
                .ForMember(x => x.IsLiked, y => y.MapFrom(x => false))
                ;

            CreateMap<AppPost, IPublicationData>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Title))
                .ForMember(x => x.Description, y => y.MapFrom(z => z.Text))
                .ForMember(x => x.GroupId, y => y.MapFrom(z => z.GroupId))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.Group.UserId))
                .ForMember(x => x.Images, y => y.MapFrom(x => x.Images.Select(x => x.Image)))
                .ForMember(x => x.Tags, y => y.Ignore())
                ;


            ///Chat
            ///
            CreateMap<IAddMessage, AppGroupMessage>()
                .ForMember(x => x.Message, y => y.MapFrom(z => z.Message))
                .ForMember(x => x.DateCreated, y => y.MapFrom(z => z.DateCreated))
                .ForMember(x => x.GroupId, y => y.MapFrom(z => z.GroupId))
                .ForMember(x => x.UserId, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Group, y => y.Ignore())
                .ForMember(x => x.User, y => y.Ignore())
                ;

            CreateMap<AppFriendMessage, FriendMessageInfo>()
                .ForMember(x => x.Message, y => y.MapFrom(z => z.Message))
                .ForMember(x => x.Date, y => y.MapFrom(z => z.DateCreated))
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserId))
                .ForMember(x => x.Image, y => y.Ignore())
                .ForMember(x => x.FullName, y => y.Ignore())
                ;

            CreateMap<AppPost, GetPostByGroupIdSorted>()
                .ForMember(x => x.Title, y => y.MapFrom(x => x.Title))
                .ForMember(x => x.Description, y => y.MapFrom(x => x.Text))
                .ForMember(x => x.Images, y => y.MapFrom(x => x.Images.Select(x => x.Image)))
                .ForMember(x => x.Tags, y => y.Ignore())
                .ForMember(x => x.UserImage, y => y.Ignore())
                .ForMember(x => x.UserEmail, y => y.Ignore())
                .ForMember(x => x.UserName, y => y.Ignore())
                .ForMember(x => x.CountLikes, y => y.Ignore())
                .ForMember(x => x.Id, y => y.MapFrom(x => x.Id))
                ;
        }
    }
}
