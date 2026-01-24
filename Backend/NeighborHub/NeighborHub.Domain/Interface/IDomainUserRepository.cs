using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Domain.Interface;
public interface IDomainUserRepository
{
    Task<DomainUser>GetDomainUserById(int userId);
    Task<DomainUser> CreateDomainUserAsync(DomainUser domainUser);
}
