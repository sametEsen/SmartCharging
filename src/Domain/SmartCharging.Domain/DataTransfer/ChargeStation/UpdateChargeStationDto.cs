using SmartCharging.Domain.DataTransfer.Connector;

namespace SmartCharging.Domain.DataTransfer.ChargeStation
{
	public class UpdateChargeStationDto
	{
		public string Name { get; set; } = string.Empty;

		public int GroupId { get; set; }

		public IEnumerable<CreateConnectorDto> Connectors { get; set; } = [];
	}
}
