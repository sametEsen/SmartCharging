using AutoMapper;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer;
using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;

namespace SmartCharging.Application.Services.Concrete
{
	public class ConnectorService : IConnectorService
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public ConnectorService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_uow = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ConnectorDto> CreateConnectorAsync(ConnectorDto connectorDto)
		{
			var connector = _mapper.Map<Connector>(connectorDto);
			await _uow.ConnectorRepository.AddAsync(connector);
			await _uow.SaveChangesAsync();
			return _mapper.Map<ConnectorDto>(connector);
		}

		public async Task<IEnumerable<ConnectorDto>> GetAllConnectorsAsync()
		{
			var connectors = await _uow.ConnectorRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ConnectorDto>>(connectors);
		}

		public async Task<ConnectorDto> GetConnectorByIdAsync(int connectorId, int id)
		{
			var connector = await _uow.ConnectorRepository.GetByIdAsync(connectorId, id);
			if (connector == null) throw new KeyNotFoundException("Connector cannot found for the charging station!");

			return _mapper.Map<ConnectorDto>(connector);
		}

		public async Task<ConnectorDto> UpdateConnectorAsync(ConnectorDto connectorDto)
		{
			var connector = await _uow.ConnectorRepository.GetByIdAsync(connectorDto.ChargeStationId, connectorDto.Id);
			if (connector == null) throw new KeyNotFoundException("Connector cannot found for the charging station!");

			_mapper.Map(connectorDto, connector);
			_uow.ConnectorRepository.Update(connector);
			await _uow.SaveChangesAsync();
			return _mapper.Map<ConnectorDto>(connector);
		}

		public async Task<bool> DeleteConnectorAsync(int chargeStationId, int id)
		{
			var connector = await _uow.ConnectorRepository.GetByIdAsync(chargeStationId, id);
			if (connector == null) throw new KeyNotFoundException("Connector cannot found for the charging station");

			_uow.ConnectorRepository.Delete(connector);
			await _uow.SaveChangesAsync();
			return true;
		}
	}
}
