using FluentValidation;
using SmartCharging.Domain.DataTransfer.ChargeStation;

namespace SmartCharging.Application.Validators.ChargeStation
{
	public class CreateChargeStationValidator : AbstractValidator<CreateChargeStationDto>
	{
		public CreateChargeStationValidator()
		{
			RuleFor(cs => cs.Name)
				.NotEmpty().WithMessage("Charge Station name is required.")
				.MaximumLength(100).WithMessage("Charge Station name must be at most 100 characters.");

			RuleFor(cs => cs.GroupId)
				.GreaterThan(0).WithMessage("A Charge Station must be associated with a valid Group.");
		}
	}
}
