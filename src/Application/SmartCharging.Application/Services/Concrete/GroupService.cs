using AutoMapper;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer;
using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;

namespace SmartCharging.Application.Services.Concrete
{
	public class GroupService : IGroupService
{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
		{
			var groups = await _unitOfWork.GroupRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<GroupDto>>(groups);
		}

		public async Task<GroupDto> GetGroupByIdAsync(int id)
		{
			var group = await _unitOfWork.GroupRepository.GetByIdAsync(id);
			return _mapper.Map<GroupDto>(group);
		}

		public async Task<GroupDto> CreateGroupAsync(GroupDto groupDto)
		{
			var group = _mapper.Map<Group>(groupDto);
			await _unitOfWork.GroupRepository.AddAsync(group);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<GroupDto>(group);
		}

		public async Task UpdateGroupAsync(GroupDto groupDto)
		{
			var group = await _unitOfWork.GroupRepository.GetByIdAsync(groupDto.Id);
			if (group == null) throw new KeyNotFoundException("Group not found");

			_mapper.Map(groupDto, group);
			_unitOfWork.GroupRepository.Update(group);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task DeleteGroupAsync(int id)
		{
			await _unitOfWork.GroupRepository.DeleteAsync(id);
			await _unitOfWork.SaveChangesAsync();
		}
	}
