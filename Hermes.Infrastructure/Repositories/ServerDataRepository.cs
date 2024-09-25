using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories;
public class ServerDataRepository : IServerDataRepository
{
    private readonly AppDbContext _context;

    public ServerDataRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddServerData(ServerData serverData)
    {
        await _context.AddAsync(serverData);

        await _context.SaveChangesAsync();
    }

    public async Task<ServerData?> GetLatestServerData()
    {
        return await _context.ServerDatas
            .OrderByDescending(sd => sd.Created)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PlayerData>> GetRecentPlayersData()
    {
        return await _context.ServerDatas
          .OrderBy(sd => sd.Created)
          .Take(24)
             .Select(sd => new PlayerData
             {
                 Players = sd.Players,
                 Created = sd.Created
             })
          .ToListAsync();
    }


    public async Task DeleteExpired()
    {
        var serverDataList = await _context.ServerDatas.ToListAsync();

        var expiredList = serverDataList.Where(d => d.IsExpired()).ToList();

        if (expiredList.Count != 0)
        {
            _context.ServerDatas.RemoveRange(expiredList);
            await _context.SaveChangesAsync();
        }
    }

}
