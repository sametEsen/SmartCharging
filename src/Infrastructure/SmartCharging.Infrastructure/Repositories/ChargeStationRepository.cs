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
	}
}
