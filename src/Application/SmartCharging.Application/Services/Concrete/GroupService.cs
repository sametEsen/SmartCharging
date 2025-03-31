using AutoMapper;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.Group;
using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;

namespace SmartCharging.Application.Services.Concrete
{
	public class GroupService : IGroupService
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_uow = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
		{
			var groups = await _uow.GroupRepository.GetAllWithRelatedChildsAsync();
			return _mapper.Map<IEnumerable<GroupDto>>(groups);
		}

		public async Task<GroupDto> GetGroupByIdAsync(int id)
		{
			var group = await _uow.GroupRepository.GetGroupWithChargeStationsAsync(id);
			if (group == null) throw new KeyNotFoundException("Group not found.");

			return _mapper.Map<GroupDto>(group);
		}

		public async Task<GroupDto> CreateGroupAsync(CreateGroupDto groupDto)
		{
			var group = new Group(0, groupDto.Name, groupDto.CapacityInAmps); // EF Core will handle Id
			await _uow.GroupRepository.AddAsync(group);
			await _uow.SaveChangesAsync();
			return _mapper.Map<GroupDto>(group);
		}

		public async Task<GroupDto> UpdateGroupAsync(int id, UpdateGroupDto updateGroupDto)
		{
			var group = await _uow.GroupRepository.GetByIdAsync(id);
			if (group == null) throw new KeyNotFoundException("Group not found.");

			if (!string.IsNullOrWhiteSpace(updateGroupDto.Name))
				group.UpdateName(updateGroupDto.Name);

			group.UpdateCapacity(updateGroupDto.CapacityInAmps);

			await _uow.SaveChangesAsync();
			return _mapper.Map<GroupDto>(group);
		}

		public async Task<bool> DeleteGroupAsync(int id)
		{
			var group = await _uow.GroupRepository.GetGroupWithChargeStationsAsync(id);
			if (group == null) 
				throw new KeyNotFoundException("Group not found.");

			_uow.GroupRepository.Delete(group);
			await _uow.SaveChangesAsync();

			return true;
		}
	}
}
