using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Domain.DataTransfer.Connector
{
	public class CreateConnectorDto
	{
		public int MaxCurrentInAmps { get; set; }
		public int ChargeStationId { get; set; }
	}

}
