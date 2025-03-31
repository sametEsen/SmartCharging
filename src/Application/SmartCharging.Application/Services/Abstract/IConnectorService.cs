using SmartCharging.Domain.DataTransfer.Connector;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IConnectorService
	{
		Task<IEnumerable<ConnectorDto>> GetAllConnectorsAsync();
		Task<ConnectorDto> GetConnectorByIdAsync(int chargeStationId, int id);
		Task<ConnectorDto> CreateConnectorAsync(CreateConnectorDto connectorDto);
		Task<ConnectorDto> UpdateConnectorAsync(int chargeStationId, int id, UpdateConnectorDto connectorDto);
		Task<bool> DeleteConnectorAsync(int chargeStationId, int id);
	}
}
