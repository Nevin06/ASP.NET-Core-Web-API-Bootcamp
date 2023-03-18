using Courseproject.Common.Dtos.Job;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class JobUpdateValidator : AbstractValidator<JobUpdate>
{
	public JobUpdateValidator()
	{
		RuleFor(jobUpdate => jobUpdate.Name).NotEmpty().MaximumLength(50);
        RuleFor(jobUpdate => jobUpdate.Description).NotEmpty().MaximumLength(250);
    }
}
