using FluentValidation;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Validators;

public class AddPurchaseRequestModelValidator : AbstractValidator<PurchaseRequestWithItemModels>
{
	public AddPurchaseRequestModelValidator()
	{
		RuleFor(x => x.ItemModels)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Item Details are required.");
	}
}
