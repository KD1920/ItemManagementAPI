using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequestModel>
{
	public ResetPasswordValidator()
	{
		RuleFor(x => x.Email)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email is invalid.")
			.MaximumLength(255).WithMessage("Email must not exceed 255 characters.");
		RuleFor(x => x.Password)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Current password is required.")
			.MinimumLength(8).WithMessage("Current password must be at least 8 characters long.")
			.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
			.WithMessage("Password must be at least 8 characters long and contain an uppercase letter, lowercase letter, number, and special character.");
		RuleFor(x => x.NewPassword)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("New password is required.")
			.MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
			.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
			.WithMessage("Password must be at least 8 characters long and contain an uppercase letter, lowercase letter, number, and special character.");
		RuleFor(x => x.ConfirmPassword)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Confirm password is required.")
			.Equal(x => x.NewPassword)
			.WithMessage("Confirm password must match the new password.");
	}
}