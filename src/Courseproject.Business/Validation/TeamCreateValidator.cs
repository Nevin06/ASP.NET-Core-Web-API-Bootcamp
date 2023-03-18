using Courseproject.Common.Dtos.Team;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class TeamCreateValidator : AbstractValidator<TeamCreate>
{
	public TeamCreateValidator()
	{
		RuleFor(teamCreate => teamCreate.Name).NotEmpty().MaximumLength(50);
    }
}
