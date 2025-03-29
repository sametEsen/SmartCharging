using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ConnectorsController : ControllerBase
	{
		private readonly IConnectorService _connectorService;

		public ConnectorsController(IConnectorService connectorService)
		{
			_connectorService = connectorService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var connectors = await _connectorService.GetAllConnectorsAsync();
			return Ok(connectors);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var connector = await _connectorService.GetConnectorByIdAsync(id);
			if (connector == null) return NotFound();
			return Ok(connector);
		}

		[HttpPost]
		public async Task<IActionResult> Create(ConnectorDto connectorDto)
		{
			var createdConnector = await _connectorService.CreateConnectorAsync(connectorDto);
			return CreatedAtAction(nameof(GetById), new { id = createdConnector.Id }, createdConnector);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, ConnectorDto connectorDto)
		{
			if (id != connectorDto.Id) return BadRequest("ID mismatch");
			await _connectorService.UpdateConnectorAsync(connectorDto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _connectorService.DeleteConnectorAsync(id);
			return NoContent();
		}
	}

}
