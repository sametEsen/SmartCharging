using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Domain.DataTransfer.Connector
{
	public class CreateConnectorDto
	{
		public int Id { get; set; } // 1-5 range
		public int MaxCurrentInAmps { get; set; }
		public int ChargeStationId { get; set; }
	}

}
