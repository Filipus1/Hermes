using System;
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

    public async Task<bool> AddServerData(ServerData serverData)
    {
        await _context.AddAsync(serverData);

        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<IEnumerable<ServerData>> GetServerData()
    {
        return await _context.ServerDatas
            .OrderBy(sd => sd.Id)
            .Take(24)
            .ToListAsync();
    }
}
