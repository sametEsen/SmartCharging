using Microsoft.EntityFrameworkCore;
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

		public async Task<IEnumerable<Group>> GetAllWithRelatedChildsAsync()
		{
			return await _dbContext.Groups
				.Include(g => g.ChargeStations)
					.ThenInclude(cs => cs.Connectors)
				.ToListAsync();
		}

		public async Task<Group?> GetGroupWithChargeStationsAsync(int id)
		{
			return await _dbContext.Groups
				.Include(g => g.ChargeStations) 
				.FirstOrDefaultAsync(g => g.Id == id);
		}
	}
}
