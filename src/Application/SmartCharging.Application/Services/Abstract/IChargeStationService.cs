using SmartCharging.Domain.DataTransfer.ChargeStation;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IChargeStationService
	{
		Task<IEnumerable<CreateChargeStationDto>> GetAllChargeStationsAsync();
		Task<CreateChargeStationDto> GetChargeStationByIdAsync(int id);
		Task<CreateChargeStationDto> CreateChargeStationAsync(CreateChargeStationDto chargeStationDto);
		Task<CreateChargeStationDto> UpdateChargeStationAsync(int id, UpdateChargeStationDto chargeStationDto);
		Task<bool> DeleteChargeStationAsync(int id);
	}
}
