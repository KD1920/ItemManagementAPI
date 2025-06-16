using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class AddUserItemRequestValidator : AbstractValidator<AddUserItemWithQuantityRequestModel>
{
	public AddUserItemRequestValidator()
	{
		RuleFor(x => x.ItemModels)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Item Details are required.");
	}
}
