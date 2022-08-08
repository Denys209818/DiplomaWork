using AutoMapper;
using Volonterio.Data.Entities.CustomEntities;
using Volonterio.Models;

namespace Volonterio.Mappers
{
    public class PostMapper : Profile
    {
        public PostMapper()
        {
            CreateMap<CreatePublicationModel, AppPost>()
                .ForMember(x => x.Title, x => x.MapFrom(y => y.Title))
                .ForMember(x => x.Text, x => x.MapFrom(y => y.Text))
                .ForMember(x => x.Images, x=> x.Ignore())
                .ForMember(x => x.PostTagEntities, x=> x.Ignore())
                .ForMember(x => x.GroupId, x=> x.Ignore())
                .ForMember(x => x.Group, x=> x.Ignore())
                .ForMember(x => x.Id, x=> x.Ignore());
        }
    }
}
