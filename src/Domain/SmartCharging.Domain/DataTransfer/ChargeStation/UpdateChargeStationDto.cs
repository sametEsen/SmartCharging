using SmartCharging.Domain.DataTransfer.Connector;

namespace SmartCharging.Domain.DataTransfer.ChargeStation
{
	public class UpdateChargeStationDto
	{
		public string Name { get; set; }
		public int GroupId { get; set; }
		public IList<CreateConnectorDto> Connectors { get; set; }
	}
}
