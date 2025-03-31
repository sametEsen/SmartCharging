using SmartCharging.Domain.Entities;

namespace SmartCharging.Domain.Interfaces
{
	public interface IGroupRepository : IGenericRepository<Group>
	{
		Task<IEnumerable<Group>> GetAllWithRelatedChildsAsync();
		Task<Group?> GetGroupWithChargeStationsAsync(int id);
	}
}
