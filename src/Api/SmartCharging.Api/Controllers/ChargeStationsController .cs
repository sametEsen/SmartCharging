using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.ChargeStation;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChargeStationsController : ControllerBase
	{
		private readonly IChargeStationService _chargeStationService;
		private readonly IValidator<CreateChargeStationDto> _createChargeStationValidator;
		private readonly IValidator<UpdateChargeStationDto> _updateChargeStationValidator;

		public ChargeStationsController(IChargeStationService chargeStationService, IValidator<CreateChargeStationDto> createChargeStationValidator, IValidator<UpdateChargeStationDto> updateChargeStationValidator)
		{
			_chargeStationService = chargeStationService;
			_createChargeStationValidator = createChargeStationValidator;
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
		public async Task<IActionResult> CreateChargeStation([FromBody] CreateChargeStationDto createDto)
		{
			var validationResult = await _createChargeStationValidator.ValidateAsync(createDto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var createdChargeStation = await _chargeStationService.CreateChargeStationAsync(createDto);
			return CreatedAtAction(nameof(GetChargeStation), new { id = createdChargeStation.Id }, createdChargeStation);
		}

		// Update an existing Charge Station
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateChargeStation(int id, [FromBody] UpdateChargeStationDto updateDto)
		{
			var validationResult = await _updateChargeStationValidator.ValidateAsync(updateDto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var updatedChargeStation = await _chargeStationService.UpdateChargeStationAsync(id, updateDto);
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
