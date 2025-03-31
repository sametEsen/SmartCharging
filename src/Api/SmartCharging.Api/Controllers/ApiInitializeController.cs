using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.ChargeStation;
using SmartCharging.Domain.DataTransfer.Connector;
using SmartCharging.Domain.DataTransfer.Group;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ApiInitializeController : ControllerBase
	{
		private readonly IGroupService _groupService;
		private readonly IConnectorService _connectorService;
		private readonly IChargeStationService _chargeStationService;

		public ApiInitializeController(
			IGroupService groupService,
			IConnectorService connectorService,
			IChargeStationService chargeStationService)
		{
			_groupService = groupService;
			_connectorService = connectorService;
			_chargeStationService = chargeStationService;
		}

		// Seeds the database
		[HttpGet]
		[Route("/seed")]
		public async Task<IActionResult> Seed()
		{
			var group = new CreateGroupDto()
			{
				Name = Guid.NewGuid().ToString(),
				CapacityInAmps = 100
			};

			var createdGroup = await _groupService.CreateGroupAsync(group);
			var chargeStation = new CreateChargeStationDto()
			{
				Name = Guid.NewGuid().ToString(),
				GroupId = createdGroup.Id
			};

			var createdChargeStation = await _chargeStationService.CreateChargeStationAsync(chargeStation);
			var connectors = new List<CreateConnectorDto>() {
				new CreateConnectorDto() { ChargeStationId = createdChargeStation.Id, MaxCurrentInAmps = 50 },
				new CreateConnectorDto() { ChargeStationId = createdChargeStation.Id, MaxCurrentInAmps = 25 },
				new CreateConnectorDto() { ChargeStationId = createdChargeStation.Id, MaxCurrentInAmps = 15 },
			};

			await Task.WhenAll(connectors.Select(c => _connectorService.CreateConnectorAsync(c)));

			return Ok();
		}
	}
}
