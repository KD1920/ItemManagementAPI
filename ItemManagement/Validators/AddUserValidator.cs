using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class AddUserValidator : AbstractValidator<AddUserRequestModel>
{
	public AddUserValidator()
	{
		RuleFor(x => x.Name)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Name is required.")
			.MinimumLength(1).WithMessage("Name must be at least 1 characters long.")
			.MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
		RuleFor(x => x.Email)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress()
			.MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
			.WithMessage("{PropertyName} is invalid! Please check!");
		RuleFor(x => x.Password)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
			.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
			.WithMessage("Password must be at least 8 characters long and contain an uppercase letter, lowercase letter, number, and special character.");
		RuleFor(x => x.RoleId)
			.GreaterThan(0)
			.WithMessage("{PropertyName} is invalid! Please check!");
		RuleFor(x => x.Active)
			.NotNull()
			.WithMessage("Active status must be provided.");
	}
}
