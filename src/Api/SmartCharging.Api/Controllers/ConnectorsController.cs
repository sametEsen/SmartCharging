using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.Connector;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ConnectorsController : ControllerBase
	{
		private readonly IConnectorService _connectorService;
		private readonly IValidator<CreateConnectorDto> _createConnectorValidator;
		private readonly IValidator<UpdateConnectorDto> _updateConnectorValidator;

		public ConnectorsController(IConnectorService connectorService, IValidator<CreateConnectorDto> createConnectorValidator, IValidator<UpdateConnectorDto> updateConnectorValidator)
		{
			_connectorService = connectorService;
			_createConnectorValidator = createConnectorValidator;
			_updateConnectorValidator = updateConnectorValidator;
		}

		// Get All Connectors
		[HttpGet]
		public async Task<IActionResult> GetAllConnectors()
		{
			var connectors = await _connectorService.GetAllConnectorsAsync();
			return Ok(connectors);
		}

		// Get Connector by ChargeStationId and Id
		[HttpGet("{chargeStationId}/{id}")]
		public async Task<IActionResult> GetConnector(int chargeStationId, int id)
		{
			var connector = await _connectorService.GetConnectorByIdAsync(chargeStationId, id);
			return connector != null ? Ok(connector) : NotFound();
		}

		// Create a new Connector
		[HttpPost]
		public async Task<IActionResult> CreateConnector([FromBody] CreateConnectorDto connectordto)
		{
			var validationResult = await _createConnectorValidator.ValidateAsync(connectordto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var createdConnector = await _connectorService.CreateConnectorAsync(connectordto);
			return CreatedAtAction(nameof(GetConnector), new { chargeStationId = connectordto.ChargeStationId, id = connectordto.Id }, createdConnector);
		}

		// Update an existing Connector
		[HttpPut("{chargeStationId}/{id}")]
		public async Task<IActionResult> UpdateConnector(int chargeStationId, int id, [FromBody] UpdateConnectorDto connectorDto)
		{
			var validationResult = await _updateConnectorValidator.ValidateAsync(connectorDto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			try
			{
				var updatedConnector = await _connectorService.UpdateConnectorAsync(chargeStationId, id, connectorDto);
				return Ok(updatedConnector);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message); // Handles domain-level validation exceptions
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}


		// Delete a Connector
		[HttpDelete("{chargeStationId}/{id}")]
		public async Task<IActionResult> DeleteConnector(int chargeStationId, int id)
		{
			var deleted = await _connectorService.DeleteConnectorAsync(chargeStationId, id);
			return deleted ? NoContent() : NotFound();
		}
	}

}
