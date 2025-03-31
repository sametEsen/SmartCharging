using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Domain.DataTransfer.ChargeStation
{
	public class CreateChargeStationDto
	{
		public string Name { get; set; }
		public int GroupId { get; set; }
	}
}
