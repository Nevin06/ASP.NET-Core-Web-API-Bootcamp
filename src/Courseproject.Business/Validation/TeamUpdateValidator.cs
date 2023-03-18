using Courseproject.Common.Dtos.Team;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class TeamUpdateValidator : AbstractValidator<TeamUpdate>
{
	public TeamUpdateValidator()
	{
		RuleFor(teamUpdate => teamUpdate.Name).NotEmpty().MaximumLength(50);
    }
}
