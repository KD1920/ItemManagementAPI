using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class AddItemTypeValidator : AbstractValidator<AddItemTypeRequestModel>
{
	public AddItemTypeValidator()
	{
		RuleFor(x => x.Name)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Name is required.")
			.MinimumLength(1).WithMessage("Name must be at least 1 characters long.")
			.MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
	}
}
