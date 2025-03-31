namespace SmartCharging.Domain.DataTransfer.Group
{
	public class UpdateGroupDto
	{
		public string Name { get; set; } = string.Empty;

		public int CapacityInAmps { get; set; }
	}
}
