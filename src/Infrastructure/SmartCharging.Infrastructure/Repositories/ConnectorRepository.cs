using Microsoft.EntityFrameworkCore;
using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;
using SmartCharging.Infrastructure.Contexts;

namespace SmartCharging.Infrastructure.Repositories
{
	public class ConnectorRepository : GenericRepository<Connector>, IConnectorRepository
	{
		public ConnectorRepository(SmartChargingContext dbContext) : base(dbContext)
		{
		}

		public async Task<Connector?> GetConnectorWithChargeStationAsync(int chargeStationId, int id)
		{
			return await _dbContext.Connectors
				.Include(c => c.ChargeStation)
				.FirstOrDefaultAsync(c => c.ChargeStationId == chargeStationId && c.Id == id);
		}
	}
}
