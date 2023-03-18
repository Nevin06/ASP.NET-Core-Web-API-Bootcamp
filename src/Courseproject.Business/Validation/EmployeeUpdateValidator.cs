using Courseproject.Common.Dtos.Employee;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class EmployeeUpdateValidator : AbstractValidator<EmployeeUpdate>
{
	public EmployeeUpdateValidator()
	{
		RuleFor(employeeCreate => employeeCreate.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(employeeCreate => employeeCreate.LastName).NotEmpty().MaximumLength(50);
    }
}
