using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IConnectorService
	{
		Task<IEnumerable<ConnectorDto>> GetAllConnectorsAsync();
		Task<ConnectorDto> GetConnectorByIdAsync(int chargeStationId, int id);
		Task<ConnectorDto> CreateConnectorAsync(ConnectorDto connectorDto);
		Task<ConnectorDto> UpdateConnectorAsync(ConnectorDto connectorDto);
		Task<bool> DeleteConnectorAsync(int chargeStationId, int id);
	}
}
