using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Domain.DataTransfer
{
	public class GroupDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CapacityInAmps { get; set; }
	}
}
