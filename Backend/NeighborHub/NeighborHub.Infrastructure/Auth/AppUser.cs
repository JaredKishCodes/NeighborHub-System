using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NeighborHub.Infrastructure.Auth;
public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string Baranggay { get; set; }   
}
