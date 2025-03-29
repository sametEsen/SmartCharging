using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IGroupService
	{
		Task<IEnumerable<GroupDto>> GetAllGroupsAsync();
		Task<GroupDto> GetGroupByIdAsync(int id);
		Task<GroupDto> CreateGroupAsync(GroupDto groupDto);
		Task UpdateGroupAsync(GroupDto groupDto);
		Task DeleteGroupAsync(int id);
	}
}
