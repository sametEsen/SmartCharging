using SmartCharging.Domain.Entities;

namespace SmartCharging.Domain.Interfaces
{
	public interface IConnectorRepository : IGenericRepository<Connector>
	{
		Task<Connector?> GetConnectorWithChargeStationAsync(int chargeStationId, int id);
	}
}
