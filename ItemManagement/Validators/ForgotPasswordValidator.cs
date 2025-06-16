using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequestModel>
{
	public ForgotPasswordValidator()
	{
		RuleFor(x => x.Email)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email is invalid.")
			.MaximumLength(255).WithMessage("Email must not exceed 255 characters.");
	}
}