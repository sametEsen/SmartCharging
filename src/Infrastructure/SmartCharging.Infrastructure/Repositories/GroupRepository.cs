using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;
using SmartCharging.Infrastructure.Contexts;

namespace SmartCharging.Infrastructure.Repositories
{
	public class GroupRepository : GenericRepository<Group>, IGroupRepository
	{
		public GroupRepository(SmartChargingContext dbContext) : base(dbContext)
		{
		}
	}
}
