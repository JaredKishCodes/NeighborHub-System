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
    Task<int> GetTotalBookingsCountAsync(int? userId = null);
    Task<int> GetTotalLendingsCountAsync(int? userId = null);
    Task<IEnumerable<Booking>> GetBookingsForCurrentMonthAsync(int? userId = null);
    Task<IEnumerable<Booking>> GetLendingsForCurrentMonthAsync(int? userId = null);


}
