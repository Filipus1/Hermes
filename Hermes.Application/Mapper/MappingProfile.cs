using AutoMapper;
using Hermes.Application.Entities;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Mapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, CollaboratorDto>();
        CreateMap<RegisterDto, User>();
    }
}