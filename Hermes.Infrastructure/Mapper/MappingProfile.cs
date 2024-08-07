using AutoMapper;
using Hermes.Application.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, CollaboratorDto>();
    }
}