using FluentValidation;
using SmartCharging.Domain.DataTransfer.Group;

namespace SmartCharging.Application.Validators.Group
{
	public class UpdateGroupValidator : AbstractValidator<UpdateGroupDto>
	{
		public UpdateGroupValidator()
		{
			RuleFor(g => g.Name)
				.NotEmpty().WithMessage("Group name is required.")
				.MaximumLength(100).WithMessage("Group name must be at most 100 characters.");

			RuleFor(g => g.CapacityInAmps)
				.GreaterThan(0).WithMessage("Capacity must be greater than zero.");
		}
	}
}
