using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Services.Abstract
{
	public interface IChargeStationService
	{
		Task<IEnumerable<ChargeStationDto>> GetAllChargeStationsAsync();
		Task<ChargeStationDto> GetChargeStationByIdAsync(int id);
		Task<ChargeStationDto> CreateChargeStationAsync(ChargeStationDto chargeStationDto);
		Task<ChargeStationDto> UpdateChargeStationAsync(ChargeStationDto chargeStationDto);
		Task<bool> DeleteChargeStationAsync(int id);
	}
}
