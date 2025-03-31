using SmartCharging.Domain.Entities;

namespace SmartCharging.Domain.Interfaces
{
	public interface IChargeStationRepository : IGenericRepository<ChargeStation>
	{
		Task<IEnumerable<ChargeStation>> GetAllDetailsAsync();
		Task<ChargeStation?> GetChargeStationWithGroupAndConnectorsAsync(int id);
	}
}
