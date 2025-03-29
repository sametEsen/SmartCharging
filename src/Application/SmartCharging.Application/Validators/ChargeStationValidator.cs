using FluentValidation;
using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Validators
{
	public class ChargeStationValidator : AbstractValidator<ChargeStationDto>
	{
		public ChargeStationValidator()
		{
			RuleFor(cs => cs.Name)
				.NotEmpty().WithMessage("Charge Station name is required.")
				.MaximumLength(100).WithMessage("Charge Station name must be at most 100 characters.");

			RuleFor(cs => cs.GroupId)
				.GreaterThan(0).WithMessage("A Charge Station must be associated with a valid Group.");
		}
	}

}
