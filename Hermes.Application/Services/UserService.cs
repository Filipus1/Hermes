using AutoMapper;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Create(RegisterDto dto)
    {
        User user = _mapper.Map<User>(dto);

        await _repository.CreateUser(user);
    }

    public async Task Create(User user)
    {
        await _repository.CreateUser(user);
    }

    public async Task Delete(List<CollaboratorDto> dto)
    {
        var allUsers = await GetAll();

        var emails = dto.Select(dto => dto.Email).ToList();

        var usersToDelete = allUsers
            .Where(u => emails.Contains(u.Email))
            .ToList();

        await _repository.DeleteUsers(usersToDelete);
    }

    public async Task<User?> Get(Guid userGuid)
    {
        return await _repository.GetUserByGuid(userGuid);
    }

    public async Task<User?> Get(string email, string password)
    {
        return await _repository.GetUserByCredentials(email, password);
    }

    public async Task<User?> Get(string email)
    {
        return await _repository.GetUserByEmail(email);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _repository.GetUsers();
    }

    public async Task<IEnumerable<CollaboratorDto>> GetCollaborators()
    {
        var collaborators = await _repository.GetCollaborators();

        return collaborators.Select(c => _mapper.Map<CollaboratorDto>(c)).ToList();
    }
}