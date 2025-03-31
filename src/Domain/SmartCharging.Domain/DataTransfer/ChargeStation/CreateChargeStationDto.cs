using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Domain.DataTransfer.ChargeStation
{
	public class CreateChargeStationDto
	{
		public string Name { get; set; } = string.Empty;

		public int GroupId { get; set; }
	}
}
