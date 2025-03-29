using FluentValidation;
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
		private readonly IValidator<ChargeStationDto> _chargeStationValidator;
		private readonly IValidator<ChargeStationDto> _updateChargeStationValidator;

		public ChargeStationsController(IChargeStationService chargeStationService, IValidator<ChargeStationDto> chargeStationValidator, IValidator<ChargeStationDto> updateChargeStationValidator)
		{
			_chargeStationService = chargeStationService;
			_chargeStationValidator = chargeStationValidator;
			_updateChargeStationValidator = updateChargeStationValidator;
		}

		// Get All Charge Stations
		[HttpGet]
		public async Task<IActionResult> GetAllChargeStations()
		{
			var chargeStations = await _chargeStationService.GetAllChargeStationsAsync();
			return Ok(chargeStations);
		}

		// Get Charge Station by Id
		[HttpGet("{id}")]
		public async Task<IActionResult> GetChargeStation(int id)
		{
			var chargeStation = await _chargeStationService.GetChargeStationByIdAsync(id);
			return chargeStation != null ? Ok(chargeStation) : NotFound();
		}

		// Create a new Charge Station
		[HttpPost]
		public async Task<IActionResult> CreateChargeStation([FromBody] ChargeStationDto dto)
		{
			var validationResult = await _chargeStationValidator.ValidateAsync(dto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var createdChargeStation = await _chargeStationService.CreateChargeStationAsync(dto);
			return CreatedAtAction(nameof(GetChargeStation), new { id = createdChargeStation.Id }, createdChargeStation);
		}

		// Update an existing Charge Station
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateChargeStation(int id, [FromBody] ChargeStationDto dto)
		{
			var validationResult = await _updateChargeStationValidator.ValidateAsync(dto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var updatedChargeStation = await _chargeStationService.UpdateChargeStationAsync(dto);
			return updatedChargeStation != null ? Ok(updatedChargeStation) : NotFound();
		}

		// Delete a Charge Station
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteChargeStation(int id)
		{
			var deleted = await _chargeStationService.DeleteChargeStationAsync(id);
			return deleted ? NoContent() : NotFound();
		}
	}

}
