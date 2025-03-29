using AutoMapper;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer;
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

		public async Task<IEnumerable<ChargeStationDto>> GetAllChargeStationsAsync()
		{
			var stations = await _uow.ChargeStationRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ChargeStationDto>>(stations);
		}

		public async Task<ChargeStationDto?> GetChargeStationByIdAsync(int id)
		{
			var station = await _uow.ChargeStationRepository.GetByIdAsync(id);
			return station == null ? null : _mapper.Map<ChargeStationDto>(station);
		}

		public async Task<ChargeStationDto> CreateChargeStationAsync(ChargeStationDto chargeStationDto)
		{
			var station = _mapper.Map<ChargeStation>(chargeStationDto);
			await _uow.ChargeStationRepository.AddAsync(station);
			await _uow.SaveChangesAsync();
			return _mapper.Map<ChargeStationDto>(station);
		}

		public async Task<ChargeStationDto> UpdateChargeStationAsync(ChargeStationDto chargeStationDto)
		{
			var chargeStation = await _uow.ChargeStationRepository.GetByIdAsync(chargeStationDto.Id);
			if (chargeStation == null) throw new KeyNotFoundException("Charge Station not found");

			_mapper.Map(chargeStationDto, chargeStation);
			_uow.ChargeStationRepository.Update(chargeStation);
			await _uow.SaveChangesAsync();
			return _mapper.Map<ChargeStationDto>(chargeStation);
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
