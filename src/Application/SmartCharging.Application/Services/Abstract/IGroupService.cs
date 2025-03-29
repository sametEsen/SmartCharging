using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IGroupService
	{
		Task<IEnumerable<CreateGroupDto>> GetAllGroupsAsync();
		Task<CreateGroupDto> GetGroupByIdAsync(int id);
		Task<CreateGroupDto> CreateGroupAsync(CreateGroupDto groupDto);
		Task<CreateGroupDto> UpdateGroupAsync(int id, UpdateGroupDto updateGroupDto);
		Task<bool> DeleteGroupAsync(int id);
	}
}
