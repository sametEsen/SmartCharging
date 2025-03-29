using SmartCharging.Domain.Interfaces;
using SmartCharging.Infrastructure.Contexts;

namespace SmartCharging.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly SmartChargingContext _dbContext;

		public IGroupRepository GroupRepository { get; }

		public IConnectorRepository ConnectorRepository { get; }

		public IChargeStationRepository ChargeStationRepository { get; }

		public UnitOfWork(SmartChargingContext dbContext)
		{
			_dbContext = dbContext;
			GroupRepository = new GroupRepository(_dbContext);
			ConnectorRepository = new ConnectorRepository(_dbContext);
			ChargeStationRepository = new ChargeStationRepository(_dbContext);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_dbContext.Dispose();
			}
		}

		public async Task<int> SaveChangesAsync()
		{
			return await this._dbContext.SaveChangesAsync();
		}
	}
}
