namespace SmartCharging.Domain.DataTransfer
{
	public class CreateGroupDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CapacityInAmps { get; set; }
	}
}
