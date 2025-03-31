using FluentValidation;
using SmartCharging.Domain.DataTransfer.Group;

namespace SmartCharging.Application.Validators.Group
{
	public class CreateGroupValidator : AbstractValidator<CreateGroupDto>
	{
		public CreateGroupValidator()
		{
			RuleFor(g => g.Name)
				.NotEmpty().WithMessage("Group name is required.");

			RuleFor(g => g.CapacityInAmps)
				.GreaterThan(0).WithMessage("Capacity must be greater than zero.");
		}
	}
}
