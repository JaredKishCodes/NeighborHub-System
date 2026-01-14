using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Domain.Interface;
public interface IDashboardRepository
{
    Task<int> GetTotalUsersCountAsync();
    Task<int> GetTotalToolsCountAsync();
    Task<int> GetTotalBookingsCountAsync();
    Task<int> GetTotalLendingsCountAsync();
    Task<int> GetTotalBookingsThisMonthAsync();
    Task<IEnumerable<Booking>> GetBookingsForCurrentMonthAsync();
    Task<IEnumerable<Booking>> GetLendingsForCurrentMonthAsync();


}
