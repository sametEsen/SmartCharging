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

		public async Task<IEnumerable<CreateGroupDto>> GetAllGroupsAsync()
		{
			var groups = await _uow.GroupRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CreateGroupDto>>(groups);
		}

		public async Task<CreateGroupDto> GetGroupByIdAsync(int id)
		{
			var group = await _uow.GroupRepository.GetByIdAsync(id);
			if (group == null) throw new KeyNotFoundException("Group not found.");

			return _mapper.Map<CreateGroupDto>(group);
		}

		public async Task<CreateGroupDto> CreateGroupAsync(CreateGroupDto groupDto)
		{
			var group = new Group(0, groupDto.Name, groupDto.CapacityInAmps); // ID will be set by DB
			await _uow.GroupRepository.AddAsync(group);
			await _uow.SaveChangesAsync();
			return _mapper.Map<CreateGroupDto>(group);
		}

		public async Task<CreateGroupDto> UpdateGroupAsync(int id, UpdateGroupDto updateGroupDto)
		{
			var group = await _uow.GroupRepository.GetByIdAsync(id);
			if (group == null) throw new KeyNotFoundException("Group not found.");

			if (!string.IsNullOrWhiteSpace(updateGroupDto.Name))
				group.UpdateName(updateGroupDto.Name);

			group.UpdateCapacity(updateGroupDto.CapacityInAmps);

			await _uow.SaveChangesAsync();
			return _mapper.Map<CreateGroupDto>(group);
		}

		public async Task<bool> DeleteGroupAsync(int id)
		{
			var group = await _uow.GroupRepository.GetGroupWithChargeStationsAsync(id);
			if (group == null) throw new KeyNotFoundException("Group not found.");

			// Remove all charge stations in the group
			foreach (var chargeStation in group.ChargeStations.ToList()) // ToList() avoids collection modification issues
			{
				_uow.ChargeStationRepository.Delete(chargeStation);
			}

			// Now delete the group itself
			_uow.GroupRepository.Delete(group);
			await _uow.SaveChangesAsync();

			return true;
		}

	}
}
