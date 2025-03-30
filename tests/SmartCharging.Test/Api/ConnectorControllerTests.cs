using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartCharging.Api.Controllers;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Domain.DataTransfer.ChargeStation;
using SmartCharging.Domain.DataTransfer.Connector;
using SmartCharging.Domain.DataTransfer.Group;
using SmartCharging.Domain.Entities;

namespace SmartCharging.Tests.Api
{
	[TestFixture]
	public class ConnectorControllerTests
	{
		private Mock<IGroupService> _groupServiceMock;
		private Mock<IConnectorService> _connectorServiceMock;
		private Mock<IChargeStationService> _chargeStationServiceMock;
		private Mock<IValidator<CreateConnectorDto>> _createValidatorMock;
		private Mock<IValidator<UpdateConnectorDto>> _updateValidatorMock;
		private ConnectorsController _controller;

		[SetUp]
		public void SetUp()
		{
			_groupServiceMock = new Mock<IGroupService>();
			_connectorServiceMock = new Mock<IConnectorService>();
			_chargeStationServiceMock = new Mock<IChargeStationService>();
			_createValidatorMock = new Mock<IValidator<CreateConnectorDto>>();
			_updateValidatorMock = new Mock<IValidator<UpdateConnectorDto>>();

			_controller = new ConnectorsController(
				_connectorServiceMock.Object,
				_createValidatorMock.Object,
				_updateValidatorMock.Object
			);
		}

		#region GetAll

		[Test]
		public async Task GetAll_ShouldReturnListOfConnectors()
		{
			// Arrange
			var connectors = new List<CreateConnectorDto>
			{
				new CreateConnectorDto { ChargeStationId = 1, Id = 1, MaxCurrentInAmps = 50 },
				new CreateConnectorDto { ChargeStationId = 1, Id = 2, MaxCurrentInAmps = 30 }
			};
			_connectorServiceMock.Setup(service => service.GetAllConnectorsAsync()).ReturnsAsync(connectors);

			// Act
			var result = await _controller.GetAllConnectors() as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(connectors);
		}

		#endregion

		#region GetById

		[Test]
		public async Task GetById_ValidId_ShouldReturnConnector()
		{
			// Arrange
			var chargeStationId = 1;
			var id = 1;
			var connector = new CreateConnectorDto { ChargeStationId = chargeStationId, Id = id, MaxCurrentInAmps = 50 };
			_connectorServiceMock.Setup(service => service.GetConnectorByIdAsync(1, 1)).ReturnsAsync(connector);

			// Act
			var result = await _controller.GetConnector(chargeStationId, id) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(connector);
		}

		[Test]
		public async Task GetById_InvalidId_ShouldReturnNotFound()
		{
			// Arrange
			_connectorServiceMock.Setup(service => service.GetConnectorByIdAsync(1, 99)).ReturnsAsync((CreateConnectorDto)null);

			// Act
			var result = await _controller.GetConnector(1, 99);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		#endregion

		#region Create

		[Test]
		public async Task Create_ValidDto_ShouldReturnCreatedConnector()
		{
			// Arrange
			var createDto = new CreateConnectorDto { ChargeStationId = 1, Id = 3, MaxCurrentInAmps = 40 };
			var createdDto = new CreateConnectorDto { ChargeStationId = 1, Id = 3, MaxCurrentInAmps = 40 };

			_createValidatorMock.Setup(v => v.ValidateAsync(createDto, default))
				.ReturnsAsync(new ValidationResult());

			_connectorServiceMock.Setup(service => service.CreateConnectorAsync(createDto)).ReturnsAsync(createdDto);

			// Act
			var result = await _controller.CreateConnector(createDto) as CreatedAtActionResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(201);
			result.Value.Should().BeEquivalentTo(createdDto);
		}

		[Test]
		public async Task Create_InvalidDto_ShouldReturnBadRequest()
		{
			// Arrange
			var createDto = new CreateConnectorDto { ChargeStationId = 1, Id = 3, MaxCurrentInAmps = 0 };
			var validationErrors = new ValidationResult(new List<ValidationFailure>
			{
				new ValidationFailure("MaxCurrentInAmps", "MaxCurrentInAmps must be greater than zero.")
			});

			_createValidatorMock.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(validationErrors);

			// Act
			var result = await _controller.CreateConnector(createDto) as BadRequestObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(400);
			result.Value.Should().BeEquivalentTo(validationErrors.Errors);
		}

		#endregion

		#region Update

		[Test]
		public async Task Update_ValidDto_ShouldReturnUpdatedConnector()
		{
			// Arrange
			var updateDto = new UpdateConnectorDto { MaxCurrentInAmps = 45 };
			var updatedDto = new CreateConnectorDto { ChargeStationId = 1, Id = 1, MaxCurrentInAmps = 45 };

			_updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, default))
				.ReturnsAsync(new ValidationResult());

			_connectorServiceMock.Setup(service => service.UpdateConnectorAsync(1, 1, updateDto)).ReturnsAsync(updatedDto);

			// Act
			var result = await _controller.UpdateConnector(1, 1, updateDto) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(updatedDto);
		}

		[Test]
		public async Task Update_InvalidDto_ShouldReturnBadRequest()
		{
			// Arrange
			var updateDto = new UpdateConnectorDto { MaxCurrentInAmps = -10 };
			var validationErrors = new ValidationResult(new List<ValidationFailure>
			{
				new ValidationFailure("MaxCurrentInAmps", "MaxCurrentInAmps must be greater than zero.")
			});

			_updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(validationErrors);

			// Act
			var result = await _controller.UpdateConnector(1, 1, updateDto) as BadRequestObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(400);
			result.Value.Should().BeEquivalentTo(validationErrors.Errors);
		}

		[Test]
		public async Task UpdateConnector_MaxCurrentExceedsGroupCapacity_ShouldReturnBadRequest()
		{
			// Arrange
			var updateConnectorDto = new UpdateConnectorDto { MaxCurrentInAmps = 80 }; // This would exceed group's capacity!

			_updateValidatorMock
				.Setup(v => v.ValidateAsync(updateConnectorDto, default))
				.ReturnsAsync(new ValidationResult()); // Assume validation passes

			_connectorServiceMock
				.Setup(s => s.UpdateConnectorAsync(1, 1, updateConnectorDto))
				.ThrowsAsync(new InvalidOperationException("Updating max current exceeds the group's capacity."));

			// Act
			var result = await _controller.UpdateConnector(1, 1, updateConnectorDto) as BadRequestObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(400);
			result.Value.Should().Be("Updating max current exceeds the group's capacity.");
		}


		#endregion

		#region Delete

		[Test]
		public async Task Delete_ValidId_ShouldReturnNoContent()
		{
			// Arrange
			_connectorServiceMock.Setup(service => service.DeleteConnectorAsync(1, 1)).ReturnsAsync(true);

			// Act
			var result = await _controller.DeleteConnector(1, 1);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		[Test]
		public async Task Delete_InvalidId_ShouldReturnNotFound()
		{
			// Arrange
			_connectorServiceMock.Setup(service => service.DeleteConnectorAsync(1, 99)).ReturnsAsync(false);

			// Act
			var result = await _controller.DeleteConnector(1, 99);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		#endregion
	}
}
