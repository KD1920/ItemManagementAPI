using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class AddItemModelValidator : AbstractValidator<AddItemModelRequestModel>
{
	public AddItemModelValidator()
	{
		RuleFor(x => x.Name)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Name is required.")
			.MinimumLength(1).WithMessage("Name must be at least 1 characters long.")
			.MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
		RuleFor(x => x.ItemTypeId)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("ItemTypeId is required.")
			.GreaterThan(0)
			.WithMessage("{PropertyName} is invalid! Please check!");
	}
}
