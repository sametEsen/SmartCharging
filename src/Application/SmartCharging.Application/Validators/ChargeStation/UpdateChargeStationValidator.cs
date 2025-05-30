﻿using FluentValidation;
using SmartCharging.Domain.DataTransfer.ChargeStation;

namespace SmartCharging.Application.Validators.ChargeStation
{
	public class UpdateChargeStationValidator : AbstractValidator<UpdateChargeStationDto>
	{
		public UpdateChargeStationValidator()
		{
			RuleFor(cs => cs.Name)
				.NotEmpty().WithMessage("Charge Station name is required.");

			RuleFor(cs => cs.GroupId)
				.GreaterThan(0).WithMessage("A Charge Station must be associated with a valid Group.");
		}
	}
}
