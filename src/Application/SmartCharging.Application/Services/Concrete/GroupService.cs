using AutoMapper;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer;
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
			return _mapper.Map<CreateGroupDto>(group);
		}

		public async Task<CreateGroupDto> CreateGroupAsync(CreateGroupDto groupDto)
		{
			var group = _mapper.Map<Group>(groupDto);
			await _uow.GroupRepository.AddAsync(group);
			await _uow.SaveChangesAsync();
			return _mapper.Map<CreateGroupDto>(group);
		}

		public async Task<CreateGroupDto> UpdateGroupAsync(int id, UpdateGroupDto updateGroupDto)
		{
			var group = await _uow.GroupRepository.GetByIdAsync(id);
			if (group == null) throw new KeyNotFoundException("Group not found");

			_mapper.Map(updateGroupDto, group);
			_uow.GroupRepository.Update(group);
			await _uow.SaveChangesAsync();
			return _mapper.Map<CreateGroupDto>(group);
		}

		public async Task<bool> DeleteGroupAsync(int id)
		{
			var group = await _uow.GroupRepository.GetByIdAsync(id); 
			if (group == null) throw new KeyNotFoundException("Group not found");

			_uow.GroupRepository.Delete(group);
			await _uow.SaveChangesAsync();
			return true;
		}
	}
}
