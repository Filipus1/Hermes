using Hermes.Application.Entities;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Abstraction;

public interface IUserService
{
    public Task Create(RegisterDto dto);
    public Task Create(User user);
    public Task Delete(List<CollaboratorDto> dto);
    public Task<User?> GetUserbyGuid(Guid userGuid);
    public Task<User?> GetUser(UserDto dto);
    public Task<User?> GetUserByEmail(string email);
    public Task<IEnumerable<CollaboratorDto>> GetCollaborators();
    public Task<IEnumerable<User>> GetAll();
}