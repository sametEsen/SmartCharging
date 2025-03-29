using FluentValidation;
using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Validators
{
	public class ConnectorValidator : AbstractValidator<ConnectorDto>
	{
		public ConnectorValidator()
		{
			RuleFor(c => c.Id)
				.InclusiveBetween(1, 5).WithMessage("Connector ID must be between 1 and 5.");

			RuleFor(c => c.MaxCurrentInAmps)
				.GreaterThan(0).WithMessage("Max current must be greater than zero.");

			RuleFor(c => c.ChargeStationId)
				.GreaterThan(0).WithMessage("A Connector must belong to a valid Charge Station.");
		}
	}

}
