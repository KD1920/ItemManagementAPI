using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class AddUserItemReturnRequestValidator : AbstractValidator<AddUserItemWithQuantityRequestModel>
{
	public AddUserItemReturnRequestValidator()
	{
		RuleFor(x => x.ItemModels)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Item Details are required.");
	}
}
