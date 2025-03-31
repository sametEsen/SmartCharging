using Microsoft.EntityFrameworkCore;
using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;
using SmartCharging.Infrastructure.Contexts;

namespace SmartCharging.Infrastructure.Repositories
{
	public class ChargeStationRepository : GenericRepository<ChargeStation>, IChargeStationRepository
	{
		public ChargeStationRepository(SmartChargingContext dbContext) : base(dbContext)
		{
		}

		public async Task<IEnumerable<ChargeStation>> GetAllDetailsAsync()
		{
			return await _dbContext.ChargeStations
				.Include(g => g.Group)
				.Include(g => g.Connectors)
				.ToListAsync();
		}

		public async Task<ChargeStation?> GetChargeStationWithGroupAndConnectorsAsync(int id)
		{
			return await _dbContext.ChargeStations
				.Include(g => g.Group)
				.Include(g => g.Connectors)
				.FirstOrDefaultAsync(c => c.Id == id);
		}
	}
}
