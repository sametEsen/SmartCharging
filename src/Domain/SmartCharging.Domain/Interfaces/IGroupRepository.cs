using SmartCharging.Domain.Entities;

namespace SmartCharging.Domain.Interfaces
{
	public interface IGroupRepository : IGenericRepository<Group>
	{
		Task<Group?> GetGroupWithChargeStationsAsync(int id);
	}
}
