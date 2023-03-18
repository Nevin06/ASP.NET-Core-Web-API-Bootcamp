using Courseproject.Common.Dtos.Job;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class JobCreateValidator : AbstractValidator<JobCreate>
{
	public JobCreateValidator()
	{
		RuleFor(jobCreate => jobCreate.Name).NotEmpty().MaximumLength(50);
        RuleFor(jobCreate => jobCreate.Description).NotEmpty().MaximumLength(250);
    }
}
