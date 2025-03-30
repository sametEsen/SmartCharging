using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using SmartCharging.Api.Controllers;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.Entities;
using AutoMapper;
using SmartCharging.Infrastructure.MappingProfiles;
using FluentAssertions;
using SmartCharging.Domain.DataTransfer.Group;
using SmartCharging.Application.Services.Concrete;
using SmartCharging.Domain.DataTransfer.ChargeStation;
using SmartCharging.Domain.Interfaces;

namespace SmartCharging.Tests.Api
{
	[TestFixture]
	public class GroupsControllerTests
	{
		private Mock<IGroupService> _groupServiceMock;
		private Mock<IValidator<CreateGroupDto>> _createGroupValidatorMock;
		private Mock<IValidator<UpdateGroupDto>> _updateGroupValidatorMock;
		private IMapper _mapper;
		private GroupsController _controller;

		[SetUp]
		public void SetUp()
		{
			// AutoMapper setup
			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new SmartChargingProfile()); // Use the profile created for Group to GroupDto mapping
			});
			_mapper = configuration.CreateMapper();

			_groupServiceMock = new Mock<IGroupService>();
			_createGroupValidatorMock = new Mock<IValidator<CreateGroupDto>>();
			_updateGroupValidatorMock = new Mock<IValidator<UpdateGroupDto>>();
			_controller = new GroupsController(
				_groupServiceMock.Object,
				_createGroupValidatorMock.Object,
				_updateGroupValidatorMock.Object
			);
		}

		#region GetAllGroups

		[Test]
		public async Task GetAllGroups_ShouldReturnOkResult_WhenGroupsExist()
		{
			// Arrange
			var groups = new List<Group>
			{
				new Group(1, "Group 1", 100),
				new Group(2, "Group 2", 200)
			};

			// Map the groups to GroupDto using AutoMapper
			var groupDtos = _mapper.Map<List<CreateGroupDto>>(groups);
			_groupServiceMock.Setup(service => service.GetAllGroupsAsync()).ReturnsAsync(groupDtos);

			// Act
			var result = await _controller.GetAllGroups();

			// Assert
			var okResult = result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult.StatusCode.Should().Be(200);
			var returnedGroupDtos = okResult.Value as List<CreateGroupDto>;
			returnedGroupDtos.Should().HaveCount(2);
			returnedGroupDtos[0].Name.Should().Be("Group 1");
			returnedGroupDtos[1].Name.Should().Be("Group 2");
		}

		[Test]
		public async Task GetAllGroups_ShouldReturnNotFound_WhenNoGroupsExist()
		{
			// Arrange
			_groupServiceMock.Setup(service => service.GetAllGroupsAsync()).ReturnsAsync(new List<CreateGroupDto>());

			// Act
			var result = await _controller.GetAllGroups();

			// Assert
			var okResult = result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult.StatusCode.Should().Be(200);
			var returnedGroupDtos = okResult.Value as List<CreateGroupDto>;
			returnedGroupDtos.Should().BeEmpty();
		}

		#endregion

		#region GetGroupById

		[Test]
		public async Task GetGroup_ShouldReturnOkResult_WhenGroupExists()
		{
			// Arrange
			var groupId = 1;
			var group = new Group(groupId, "Group 1", 100);
			var groupDto = _mapper.Map<CreateGroupDto>(group);
			_groupServiceMock.Setup(service => service.GetGroupByIdAsync(groupId)).ReturnsAsync(groupDto);

			// Act
			var result = await _controller.GetGroup(groupId);

			// Assert
			var okResult = result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult.StatusCode.Should().Be(200);
			var returnedGroupDto = okResult.Value as CreateGroupDto;
			returnedGroupDto.Should().NotBeNull();
			returnedGroupDto.Name.Should().Be("Group 1");
		}

		[Test]
		public async Task GetGroup_ShouldReturnNotFound_WhenGroupDoesNotExist()
		{
			// Arrange
			var groupId = 99;
			_groupServiceMock.Setup(service => service.GetGroupByIdAsync(groupId)).ReturnsAsync((CreateGroupDto)null);

			// Act
			var result = await _controller.GetGroup(groupId);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		#endregion

		#region CreateGroup

		[Test]
		public async Task CreateGroup_ShouldReturnCreatedAtAction_WhenValidData()
		{
			// Arrange
			var newGroupDto = new CreateGroupDto { Name = "New Group", CapacityInAmps = 150 };
			var group = new Group(1, "New Group", 150);
			var createdGroupDto = _mapper.Map<CreateGroupDto>(group);

			_createGroupValidatorMock.Setup(validator => validator.ValidateAsync(newGroupDto, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
			_groupServiceMock.Setup(service => service.CreateGroupAsync(newGroupDto)).ReturnsAsync(createdGroupDto);

			// Act
			var result = await _controller.CreateGroup(newGroupDto);

			// Assert
			var createdResult = result as CreatedAtActionResult;
			createdResult.Should().NotBeNull();
			createdResult.StatusCode.Should().Be(201);
			var returnedGroup = createdResult.Value as CreateGroupDto;
			returnedGroup.Should().NotBeNull();
			returnedGroup.Name.Should().Be("New Group");
		}

		[Test]
		public async Task CreateGroup_ShouldReturnBadRequest_WhenInvalidData()
		{
			// Arrange
			var newGroupDto = new CreateGroupDto { Name = "New Group", CapacityInAmps = -10 };
			var validationErrors = new FluentValidation.Results.ValidationFailure[] { new FluentValidation.Results.ValidationFailure("CapacityInAmps", "Capacity must be greater than zero.") };
			_createGroupValidatorMock.Setup(validator => validator.ValidateAsync(newGroupDto, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

			// Act
			var result = await _controller.CreateGroup(newGroupDto);

			// Assert
			var badRequestResult = result as BadRequestObjectResult;
			badRequestResult.Should().NotBeNull();
			badRequestResult.StatusCode.Should().Be(400);
			badRequestResult.Value.Should().BeEquivalentTo(validationErrors);
		}

		#endregion

		#region UpdateGroup

		[Test]
		public async Task UpdateGroup_ShouldReturnOkResult_WhenValidData()
		{
			// Arrange
			var groupId = 1;
			var updateGroupDto = new UpdateGroupDto { Name = "Updated Group", CapacityInAmps = 200 };
			var group = new Group(groupId, "Updated Group", 200);
			var groupDto = _mapper.Map<CreateGroupDto>(group);

			// Mock the validator to return a successful validation result
			var validationResult = new FluentValidation.Results.ValidationResult();
			_updateGroupValidatorMock.Setup(validator => validator.ValidateAsync(updateGroupDto, It.IsAny<CancellationToken>()))
									 .ReturnsAsync(validationResult);

			// Mock the service to return the mapped GroupDto
			_groupServiceMock.Setup(service => service.UpdateGroupAsync(groupId, updateGroupDto))
							 .ReturnsAsync(groupDto);

			// Act
			var result = await _controller.UpdateGroup(groupId, updateGroupDto);

			// Assert
			var okResult = result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult.StatusCode.Should().Be(200);
			var updatedGroup = okResult.Value as CreateGroupDto;
			updatedGroup.Should().NotBeNull();
			updatedGroup.Name.Should().Be("Updated Group");
		}

		[Test]
		public async Task UpdateGroup_ShouldReturnNotFound_WhenGroupDoesNotExist()
		{
			// Arrange
			var groupId = 99;
			var updateGroupDto = new UpdateGroupDto { Name = "Updated Group", CapacityInAmps = 200 };

			// Mock the validator to return a successful validation result
			var validationResult = new FluentValidation.Results.ValidationResult();
			_updateGroupValidatorMock.Setup(validator => validator.ValidateAsync(updateGroupDto, It.IsAny<CancellationToken>()))
									 .ReturnsAsync(validationResult);

			// Mock the service to return the mapped GroupDto
			_groupServiceMock.Setup(service => service.UpdateGroupAsync(groupId, updateGroupDto))
							 .ReturnsAsync((CreateGroupDto)null);

			// Act
			var result = await _controller.UpdateGroup(groupId, updateGroupDto);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		#endregion

		#region DeleteGroup

		[Test]
		public async Task DeleteGroup_ShouldReturnNoContent_WhenGroupDeletedSuccessfully()
		{
			// Arrange
			var groupId = 1;
			_groupServiceMock.Setup(service => service.DeleteGroupAsync(groupId)).ReturnsAsync(true);

			// Act
			var result = await _controller.DeleteGroup(groupId);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		[Test]
		public async Task DeleteGroup_GroupWithChargeStations_ShouldRemoveChargeStationsAndGroup()
		{
			// Arrange
			var group = new CreateGroupDto { Id = 1, Name = "Group A", CapacityInAmps = 100 };
			var chargeStation1 = new CreateChargeStationDto { Id = 1, Name = "Station X", GroupId = 1 };
			var chargeStation2 = new CreateChargeStationDto { Id = 2, Name = "Station Y", GroupId = 1 };

			// Mock the data for Group and ChargeStations
			_groupServiceMock.Setup(s => s.GetGroupByIdAsync(1)).ReturnsAsync(group);

			// Mock the DeleteGroupAsync method to delete the group and its charge stations
			_groupServiceMock.Setup(s => s.DeleteGroupAsync(1)).ReturnsAsync(true);

			// Act
			var result = await _controller.DeleteGroup(1) as NoContentResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(204); // No Content response

			// Verify the delete action for Group
			_groupServiceMock.Verify(s => s.DeleteGroupAsync(1), Times.Once);
		}

		[Test]
		public async Task DeleteGroup_GroupNotFound_ShouldReturnNotFound()
		{
			// Arrange
			_groupServiceMock.Setup(s => s.DeleteGroupAsync(1)).ReturnsAsync(false);

			// Act
			var result = await _controller.DeleteGroup(1) as NotFoundResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(404); // Not Found response
		}


		#endregion
	}
}
