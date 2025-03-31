using SmartCharging.Domain.DataTransfer.ChargeStation;

namespace SmartCharging.Domain.DataTransfer.Group
{
	public class GroupDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int CapacityInAmps { get; set; }
		public IEnumerable<ChargeStationDto> ChargeStations { get; set; } = [];
	}
}
