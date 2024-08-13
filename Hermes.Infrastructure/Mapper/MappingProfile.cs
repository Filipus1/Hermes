using AutoMapper;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Dto;

namespace Hermes.Infrastracture.Mapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, CollaboratorDto>();
        CreateMap<RegisterDto, User>();
    }
}