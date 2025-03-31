using SmartCharging.Domain.DataTransfer.Group;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IGroupService
	{
		Task<IEnumerable<GroupDto>> GetAllGroupsAsync();
		Task<GroupDto> GetGroupByIdAsync(int id);
		Task<GroupDto> CreateGroupAsync(CreateGroupDto groupDto);
		Task<GroupDto> UpdateGroupAsync(int id, UpdateGroupDto updateGroupDto);
		Task<bool> DeleteGroupAsync(int id);
	}
}
