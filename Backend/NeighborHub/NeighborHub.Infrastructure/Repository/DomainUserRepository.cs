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
    public async Task<DomainUser> GetDomainUserById(int userId)
    {
      return  await _context.DomainUsers.FirstOrDefaultAsync(x => x.Id == userId);
    }
}
