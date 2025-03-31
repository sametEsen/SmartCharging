using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharging.Api.Controllers;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.ChargeStation;

namespace SmartCharging.Tests.Api
{
	[TestFixture]
	public class ChargeStationControllerTests
	{
		private Mock<IChargeStationService> _chargeStationServiceMock;
		private Mock<IValidator<CreateChargeStationDto>> _createValidatorMock;
		private Mock<IValidator<UpdateChargeStationDto>> _updateValidatorMock;
		private ChargeStationsController _controller;

		[SetUp]
		public void SetUp()
		{
			_chargeStationServiceMock = new Mock<IChargeStationService>();
			_createValidatorMock = new Mock<IValidator<CreateChargeStationDto>>();
			_updateValidatorMock = new Mock<IValidator<UpdateChargeStationDto>>();

			_controller = new ChargeStationsController(
				_chargeStationServiceMock.Object,
				_createValidatorMock.Object,
				_updateValidatorMock.Object
			);
		}

		#region GetAll

		[Test]
		public async Task GetAll_ShouldReturnListOfChargeStations()
		{
			// Arrange
			var chargeStations = new List<ChargeStationDto>
			{
				new ChargeStationDto { Id = 1, Name = "Station A", GroupId = 1 },
				new ChargeStationDto { Id = 2, Name = "Station B", GroupId = 1 }
			};
			_chargeStationServiceMock.Setup(service => service.GetAllChargeStationsAsync()).ReturnsAsync(chargeStations);

			// Act
			var result = await _controller.GetAllChargeStations() as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(chargeStations);
		}

		#endregion

		#region GetById

		[Test]
		public async Task GetById_ValidId_ShouldReturnChargeStation()
		{
			// Arrange
			var chargeStation = new ChargeStationDto { Id = 1, Name = "Station A", GroupId = 1 };
			_chargeStationServiceMock.Setup(service => service.GetChargeStationByIdAsync(1)).ReturnsAsync(chargeStation);

			// Act
			var result = await _controller.GetChargeStation(1) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(chargeStation);
		}

		[Test]
		public async Task GetById_InvalidId_ShouldReturnNotFound()
		{
			// Arrange
			_chargeStationServiceMock.Setup(service => service.GetChargeStationByIdAsync(99)).ReturnsAsync((ChargeStationDto)null);

			// Act
			var result = await _controller.GetChargeStation(99);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		#endregion

		#region Create

		[Test]
		public async Task Create_ValidDto_ShouldReturnCreatedChargeStation()
		{
			// Arrange
			var createDto = new CreateChargeStationDto { Name = "Station C", GroupId = 1 };
			var createdDto = new ChargeStationDto { Id = 3, Name = "Station C", GroupId = 1 };

			_createValidatorMock.Setup(v => v.ValidateAsync(createDto, default))
				.ReturnsAsync(new ValidationResult());

			_chargeStationServiceMock.Setup(service => service.CreateChargeStationAsync(createDto)).ReturnsAsync(createdDto);

			// Act
			var result = await _controller.CreateChargeStation(createDto) as CreatedAtActionResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(201);
			result.Value.Should().BeEquivalentTo(createdDto);
		}

		[Test]
		public async Task Create_InvalidDto_ShouldReturnBadRequest()
		{
			// Arrange
			var createDto = new CreateChargeStationDto { Name = "", GroupId = 1 };
			var validationErrors = new ValidationResult(new List<ValidationFailure>
			{
				new ValidationFailure("Name", "Name is required.")
			});

			_createValidatorMock.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(validationErrors);

			// Act
			var result = await _controller.CreateChargeStation(createDto) as BadRequestObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(400);
			result.Value.Should().BeEquivalentTo(validationErrors.Errors);
		}

		#endregion

		#region Update

		[Test]
		public async Task Update_ValidDto_ShouldReturnUpdatedChargeStation()
		{
			// Arrange
			var updateDto = new UpdateChargeStationDto { Name = "Updated Station" };
			var updatedDto = new ChargeStationDto { Id = 1, Name = "Updated Station", GroupId = 1 };

			_updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, default))
				.ReturnsAsync(new ValidationResult());

			_chargeStationServiceMock.Setup(service => service.UpdateChargeStationAsync(1, updateDto)).ReturnsAsync(updatedDto);

			// Act
			var result = await _controller.UpdateChargeStation(1, updateDto) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(updatedDto);
		}

		[Test]
		public async Task Update_InvalidDto_ShouldReturnBadRequest()
		{
			// Arrange
			var updateDto = new UpdateChargeStationDto { Name = "" };
			var validationErrors = new ValidationResult(new List<ValidationFailure>
			{
				new ValidationFailure("Name", "Name is required.")
			});

			_updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(validationErrors);

			// Act
			var result = await _controller.UpdateChargeStation(1, updateDto) as BadRequestObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(400);
			result.Value.Should().BeEquivalentTo(validationErrors.Errors);
		}

		#endregion

		#region Delete

		[Test]
		public async Task Delete_ValidId_ShouldReturnNoContent()
		{
			// Arrange
			_chargeStationServiceMock.Setup(service => service.DeleteChargeStationAsync(1)).ReturnsAsync(true);

			// Act
			var result = await _controller.DeleteChargeStation(1);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		[Test]
		public async Task Delete_InvalidId_ShouldReturnNotFound()
		{
			// Arrange
			_chargeStationServiceMock.Setup(service => service.DeleteChargeStationAsync(99)).ReturnsAsync(false);

			// Act
			var result = await _controller.DeleteChargeStation(99);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		#endregion
	}
}
