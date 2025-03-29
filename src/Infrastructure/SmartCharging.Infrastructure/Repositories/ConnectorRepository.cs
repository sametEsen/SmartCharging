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
	}
}
