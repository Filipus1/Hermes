using AutoMapper;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Dto;

namespace Hermes.Infrastructure.Mapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, CollaboratorDto>();
        CreateMap<RegisterDto, User>();
        CreateMap<ServerData, ServerDataDto>();
    }
}