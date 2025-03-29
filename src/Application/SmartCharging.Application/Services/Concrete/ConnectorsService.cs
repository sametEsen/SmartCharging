using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;

namespace SmartCharging.Application.Services.Concrete
{
	public class ConnectorsService : IConnectorService
	{
		private readonly IUnitOfWork _uow;
		public ConnectorsService(IUnitOfWork uow)
		{
			_uow = uow;
		}
	}
}
