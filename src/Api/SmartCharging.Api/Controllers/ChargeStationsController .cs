using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChargeStationsController : ControllerBase
	{
		private readonly IChargeStationService _chargeStationService;

		public ChargeStationsController(IChargeStationService chargeStationService)
		{
			_chargeStationService = chargeStationService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var chargeStations = await _chargeStationService.GetAllChargeStationsAsync();
			return Ok(chargeStations);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var chargeStation = await _chargeStationService.GetChargeStationByIdAsync(id);
			if (chargeStation == null) return NotFound();
			return Ok(chargeStation);
		}

		[HttpPost]
		public async Task<IActionResult> Create(ChargeStationDto chargeStationDto)
		{
			var createdChargeStation = await _chargeStationService.CreateChargeStationAsync(chargeStationDto);
			return CreatedAtAction(nameof(GetById), new { id = createdChargeStation.Id }, createdChargeStation);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, ChargeStationDto chargeStationDto)
		{
			if (id != chargeStationDto.Id) return BadRequest("ID mismatch");
			await _chargeStationService.UpdateChargeStationAsync(chargeStationDto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _chargeStationService.DeleteChargeStationAsync(id);
			return NoContent();
		}
	}

}
