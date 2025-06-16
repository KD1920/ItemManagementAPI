using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class UpdateUserItemRequestValidator : AbstractValidator<UpdateUserItemWithQuantityRequestModel>
{
	public UpdateUserItemRequestValidator()
	{
		RuleFor(x => x.StatusId)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Status is required.")
			.GreaterThan(0);
		RuleFor(x => x.ItemModels)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Item Details are required.");
	}
}