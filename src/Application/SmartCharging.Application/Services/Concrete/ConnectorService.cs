using AutoMapper;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.Connector;
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

		public async Task<CreateConnectorDto> CreateConnectorAsync(CreateConnectorDto connectorDto)
		{
			var chargeStation = await _uow.ChargeStationRepository.GetByIdAsync(connectorDto.ChargeStationId);
			if (chargeStation == null) throw new KeyNotFoundException("Charge Station not found.");

			var connector = new Connector(connectorDto.Id, connectorDto.MaxCurrentInAmps, chargeStation);
			chargeStation.AddConnector(connector);

			await _uow.SaveChangesAsync();
			return _mapper.Map<CreateConnectorDto>(connector);
		}

		public async Task<IEnumerable<CreateConnectorDto>> GetAllConnectorsAsync()
		{
			var connectors = await _uow.ConnectorRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CreateConnectorDto>>(connectors);
		}

		public async Task<CreateConnectorDto> GetConnectorByIdAsync(int chargeStationId, int id)
		{
			var connector = await _uow.ConnectorRepository.GetByIdAsync(chargeStationId, id);
			if (connector == null) throw new KeyNotFoundException("Connector not found for the charge station.");

			return _mapper.Map<CreateConnectorDto>(connector);
		}

		public async Task<CreateConnectorDto> UpdateConnectorAsync(int chargeStationId, int id, UpdateConnectorDto connectorDto)
		{
			var connector = await _uow.ConnectorRepository.GetByIdAsync(chargeStationId, id);
			if (connector == null) throw new KeyNotFoundException("Connector not found for the charge station.");

			connector.UpdateMaxCurrent(connectorDto.MaxCurrentInAmps);

			await _uow.SaveChangesAsync();
			return _mapper.Map<CreateConnectorDto>(connector);
		}

		public async Task<bool> DeleteConnectorAsync(int chargeStationId, int id)
		{
			var chargeStation = await _uow.ChargeStationRepository.GetByIdAsync(chargeStationId);
			if (chargeStation == null) throw new KeyNotFoundException("Charge Station not found.");

			var connector = chargeStation.Connectors.FirstOrDefault(c => c.Id == id);
			if (connector == null) throw new KeyNotFoundException("Connector not found for the charge station.");

			chargeStation.RemoveConnector(connector);
			await _uow.SaveChangesAsync();
			return true;
		}
	}
}
