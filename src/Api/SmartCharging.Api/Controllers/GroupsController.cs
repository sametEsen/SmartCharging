using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GroupsController : ControllerBase
	{
		private readonly IGroupService _groupService;

		public GroupsController(IGroupService groupService)
		{
			_groupService = groupService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var groups = await _groupService.GetAllGroupsAsync();
			return Ok(groups);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var group = await _groupService.GetGroupByIdAsync(id);
			if (group == null) return NotFound();
			return Ok(group);
		}

		[HttpPost]
		public async Task<IActionResult> Create(GroupDto groupDto)
		{
			var createdGroup = await _groupService.CreateGroupAsync(groupDto);
			return CreatedAtAction(nameof(GetById), new { id = createdGroup.Id }, createdGroup);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, GroupDto groupDto)
		{
			if (id != groupDto.Id) return BadRequest("ID mismatch");
			await _groupService.UpdateGroupAsync(groupDto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _groupService.DeleteGroupAsync(id);
			return NoContent();
		}
	}
}
