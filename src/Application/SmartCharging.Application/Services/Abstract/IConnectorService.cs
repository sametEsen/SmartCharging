using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IConnectorService
	{
		Task<IEnumerable<ConnectorDto>> GetAllConnectorsAsync();
		Task<ConnectorDto> GetConnectorByIdAsync(int id);
		Task<ConnectorDto> CreateConnectorAsync(ConnectorDto connectorDto);
		Task UpdateConnectorAsync(ConnectorDto connectorDto);
		Task DeleteConnectorAsync(int id);
	}
}
