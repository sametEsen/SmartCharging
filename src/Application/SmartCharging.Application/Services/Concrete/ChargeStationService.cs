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

		public async Task<IEnumerable<ChargeStationDto>> GetAllChargeStationsAsync()
		{
			var stations = await _uow.ChargeStationRepository.GetAllDetailsAsync();
			return _mapper.Map<IEnumerable<ChargeStationDto>>(stations);
		}

		public async Task<ChargeStationDto?> GetChargeStationByIdAsync(int id)
		{
			var station = await _uow.ChargeStationRepository.GetByIdAsync(id);
			return station == null ? null : _mapper.Map<ChargeStationDto>(station);
		}

		public async Task<ChargeStationDto> CreateChargeStationAsync(CreateChargeStationDto chargeStationDto)
		{
			var station = new ChargeStation(0, chargeStationDto.Name); // 0 as Id since EF Core will handle it

			var group = await _uow.GroupRepository.GetByIdAsync(chargeStationDto.GroupId);
			if (group == null) throw new KeyNotFoundException("Group not found.");

			station.AssignToGroup(group);

			await _uow.ChargeStationRepository.AddAsync(station);
			await _uow.SaveChangesAsync();

			return _mapper.Map<ChargeStationDto>(station);
		}

		public async Task<ChargeStationDto> UpdateChargeStationAsync(int id, UpdateChargeStationDto chargeStationDto)
		{
			var station = await _uow.ChargeStationRepository.GetChargeStationWithGroupAndConnectorsAsync(id);
			if (station == null) throw new KeyNotFoundException("Charge Station not found");

			station.UpdateName(chargeStationDto.Name);

			var group = await _uow.GroupRepository.GetByIdAsync(chargeStationDto.GroupId);
			if (group != null && (station.Group == null || station.Group.Id != group.Id)) 
			{
				station.AssignToGroup(group);
			}

			if (chargeStationDto.Connectors.Count > 0)
			{
				var connectors = _mapper.Map<List<Connector>>(chargeStationDto.Connectors);
				foreach (var connector in connectors)
				{
					if (station.Connectors.Any(c => c.Id == connector.Id) == false)
					{
						station.AddConnector(connector);
						await _uow.ConnectorRepository.AddAsync(connector);
					}
					else
					{
						station.RemoveConnector(connector);
						_uow.ConnectorRepository.Delete(connector);
					}

				}
			}

			_uow.ChargeStationRepository.Update(station);
			await _uow.SaveChangesAsync();

			return _mapper.Map<ChargeStationDto>(station);
		}

		public async Task<bool> DeleteChargeStationAsync(int id)
		{
			var station = await _uow.ChargeStationRepository.GetChargeStationWithGroupAndConnectorsAsync(id);
			if (station == null) throw new KeyNotFoundException("Charge Station not found");

			_uow.ChargeStationRepository.Delete(station);
			await _uow.SaveChangesAsync();
			return true;
		}
	}
}
