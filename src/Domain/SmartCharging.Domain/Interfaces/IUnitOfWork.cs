namespace SmartCharging.Domain.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IGroupRepository GroupRepository { get; }
		IConnectorRepository ConnectorRepository { get; }
		IChargeStationRepository ChargeStationRepository { get; }
		Task<int> SaveChangesAsync();
	}
}
