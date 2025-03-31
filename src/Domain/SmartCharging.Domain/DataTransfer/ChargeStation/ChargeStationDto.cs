using SmartCharging.Domain.DataTransfer.Connector;

namespace SmartCharging.Domain.DataTransfer.ChargeStation
{
	public class ChargeStationDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int? GroupId { get; set; }

		public IEnumerable<ConnectorDto> Connectors { get; set; } = [];
	}
}
