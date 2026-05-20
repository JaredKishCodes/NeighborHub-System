using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Infrastructure.Repository;
public class DomainUserRepository(AppDbContext _context) : IDomainUserRepository
{
    public async Task<DomainUser?> GetDomainUserById(int userId)
    {
        return await _context.DomainUsers
            .Where(x => x.Id == userId)
            .Select(x => new DomainUser
            {
                Id = x.Id,
                IdentityId = x.IdentityId,
                FullName = x.FullName
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DomainUser?> GetDomainUserByIdentityIdAsync(string identityId)
    {
        return await _context.DomainUsers
            .Where(x => x.IdentityId == identityId)
            .Select(x => new DomainUser
            {
                Id = x.Id,
                IdentityId = x.IdentityId,
                FullName = x.FullName
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DomainUser> CreateDomainUserAsync(DomainUser domainUser)
    {
        _context.DomainUsers.Add(domainUser);
       await _context.SaveChangesAsync();

        return domainUser;
    }

    public async Task<DomainUser> UpdateDomainUserAsync(DomainUser domainUser)
    {
        _context.DomainUsers.Update(domainUser);
        await _context.SaveChangesAsync();
        return domainUser;
    }
}
