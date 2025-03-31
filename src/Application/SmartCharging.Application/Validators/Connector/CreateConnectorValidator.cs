using FluentValidation;
using SmartCharging.Domain.DataTransfer.Connector;

namespace SmartCharging.Application.Validators.Connector
{
	public class CreateConnectorValidator : AbstractValidator<CreateConnectorDto>
	{
		public CreateConnectorValidator()
		{
			RuleFor(c => c.MaxCurrentInAmps)
				.GreaterThan(0).WithMessage("Max current must be greater than zero.");

			RuleFor(c => c.ChargeStationId)
				.GreaterThan(0).WithMessage("A Connector must belong to a valid Charge Station.");
		}
	}

}
