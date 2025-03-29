using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Domain.DataTransfer
{
	public class ChargeStationDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int GroupId { get; set; }
	}

}
