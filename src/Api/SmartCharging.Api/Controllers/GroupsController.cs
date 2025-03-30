using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.Group;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GroupsController : ControllerBase
	{
		private readonly IGroupService _groupService;
		private readonly IValidator<CreateGroupDto> _createGroupValidator;
		private readonly IValidator<UpdateGroupDto> _updateGroupValidator;

		public GroupsController(IGroupService groupService, IValidator<CreateGroupDto> createGroupValidator, IValidator<UpdateGroupDto> updateGroupValidator)
		{
			_groupService = groupService;
			_createGroupValidator = createGroupValidator;
			_updateGroupValidator = updateGroupValidator;
		}

		// Get All Groups
		[HttpGet]
		public async Task<IActionResult> GetAllGroups()
		{
			var groups = await _groupService.GetAllGroupsAsync();
			return Ok(groups);
		}

		// Get Group by Id
		[HttpGet("{id}")]
		public async Task<IActionResult> GetGroup(int id)
		{
			var group = await _groupService.GetGroupByIdAsync(id);
			return group != null ? Ok(group) : NotFound();
		}

		// Create a new Group
		[HttpPost]
		public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto groupDto)
		{
			var validationResult = await _createGroupValidator.ValidateAsync(groupDto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var createdGroup = await _groupService.CreateGroupAsync(groupDto);
			return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.Id }, createdGroup);
		}

		// Update an existing Group
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDto updateGroupDto)
		{
			var validationResult = await _updateGroupValidator.ValidateAsync(updateGroupDto);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var updatedGroup = await _groupService.UpdateGroupAsync(id, updateGroupDto);
			return updatedGroup != null ? Ok(updatedGroup) : NotFound();
		}

		// Delete a Group
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGroup(int id)
		{
			var deleted = await _groupService.DeleteGroupAsync(id);
			return deleted ? NoContent() : NotFound();
		}
	}

}
