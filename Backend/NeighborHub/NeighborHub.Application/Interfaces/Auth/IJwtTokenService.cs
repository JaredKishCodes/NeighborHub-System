using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Auth;

namespace NeighborHub.Application.Interfaces.Auth;
public interface IJwtTokenService
{
    Task<string> CreateToken(UserDto user, List<string> roles);
}
