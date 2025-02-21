using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;

namespace SalesOnline.Infrastructure.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
