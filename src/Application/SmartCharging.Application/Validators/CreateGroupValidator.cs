﻿using FluentValidation;
using SmartCharging.Domain.DataTransfer;

namespace SmartCharging.Application.Validators
{
	public class CreateGroupValidator : AbstractValidator<CreateGroupDto>
	{
		public CreateGroupValidator()
		{
			RuleFor(g => g.Name)
				.NotEmpty().WithMessage("Group name is required.")
				.MaximumLength(100).WithMessage("Group name must be at most 100 characters.");

			RuleFor(g => g.CapacityInAmps)
				.GreaterThan(0).WithMessage("Capacity must be greater than zero.");
		}
	}
}
