namespace SmartCharging.Domain.DataTransfer.Connector
{
	public class UpdateConnectorDto
	{
		public int Id { get; set; }

		public int MaxCurrentInAmps { get; set; }

		public int ChargeStationId { get; set; }
	}
}
