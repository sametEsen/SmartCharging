using AutoMapper;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.ChargeStation;
using SmartCharging.Domain.Entities;
using SmartCharging.Domain.Interfaces;

namespace SmartCharging.Application.Services.Concrete
{
	public class ChargeStationService : IChargeStationService
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public ChargeStationService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_uow = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CreateChargeStationDto>> GetAllChargeStationsAsync()
		{
			var stations = await _uow.ChargeStationRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CreateChargeStationDto>>(stations);
		}

		public async Task<CreateChargeStationDto?> GetChargeStationByIdAsync(int id)
		{
			var station = await _uow.ChargeStationRepository.GetByIdAsync(id);
			return station == null ? null : _mapper.Map<CreateChargeStationDto>(station);
		}

		public async Task<CreateChargeStationDto> CreateChargeStationAsync(CreateChargeStationDto chargeStationDto)
		{
			if (string.IsNullOrWhiteSpace(chargeStationDto.Name))
				throw new ArgumentException("Charge station name cannot be empty.");

			var station = new ChargeStation(0, chargeStationDto.Name); // 0 as Id since EF Core will handle it

			var group = await _uow.GroupRepository.GetByIdAsync(chargeStationDto.GroupId);
			if (group == null) throw new KeyNotFoundException("Group not found.");

			station.AssignToGroup(group);

			await _uow.ChargeStationRepository.AddAsync(station);
			await _uow.SaveChangesAsync();

			return _mapper.Map<CreateChargeStationDto>(station);
		}

		public async Task<CreateChargeStationDto> UpdateChargeStationAsync(int id, UpdateChargeStationDto chargeStationDto)
		{
			var station = await _uow.ChargeStationRepository.GetByIdAsync(id);
			if (station == null) throw new KeyNotFoundException("Charge Station not found");

			station.UpdateName(chargeStationDto.Name);

			var group = await _uow.GroupRepository.GetByIdAsync(chargeStationDto.GroupId);
			if (group != null) 
			{
				station.AssignToGroup(group);
			}
			else
			{
				station.UnassignFromGroup();
			}

			_uow.ChargeStationRepository.Update(station);
			await _uow.SaveChangesAsync();

			return _mapper.Map<CreateChargeStationDto>(station);
		}

		public async Task<bool> DeleteChargeStationAsync(int id)
		{
			var station = await _uow.ChargeStationRepository.GetByIdAsync(id);
			if (station == null) throw new KeyNotFoundException("Charge Station not found");

			_uow.ChargeStationRepository.Delete(station);
			await _uow.SaveChangesAsync();
			return true;
		}
	}
}
