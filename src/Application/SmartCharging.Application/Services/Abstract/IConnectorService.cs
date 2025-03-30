using SmartCharging.Domain.DataTransfer.Connector;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IConnectorService
	{
		Task<IEnumerable<CreateConnectorDto>> GetAllConnectorsAsync();
		Task<CreateConnectorDto> GetConnectorByIdAsync(int chargeStationId, int id);
		Task<CreateConnectorDto> CreateConnectorAsync(CreateConnectorDto connectorDto);
		Task<CreateConnectorDto> UpdateConnectorAsync(int chargeStationId, int id, UpdateConnectorDto connectorDto);
		Task<bool> DeleteConnectorAsync(int chargeStationId, int id);
	}
}
