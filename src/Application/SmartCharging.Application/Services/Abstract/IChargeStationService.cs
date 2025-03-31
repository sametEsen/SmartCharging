using SmartCharging.Domain.DataTransfer.ChargeStation;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IChargeStationService
	{
		Task<IEnumerable<ChargeStationDto>> GetAllChargeStationsAsync();
		Task<ChargeStationDto> GetChargeStationByIdAsync(int id);
		Task<ChargeStationDto> CreateChargeStationAsync(CreateChargeStationDto chargeStationDto);
		Task<ChargeStationDto> UpdateChargeStationAsync(int id, UpdateChargeStationDto chargeStationDto);
		Task<bool> DeleteChargeStationAsync(int id);
	}
}
