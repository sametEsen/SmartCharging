namespace SmartCharging.Domain.DataTransfer.Group
{
	public class CreateGroupDto
	{
		public string Name { get; set; } = string.Empty;

		public int CapacityInAmps { get; set; }
	}
}
