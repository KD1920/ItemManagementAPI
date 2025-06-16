using FluentValidation;
using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Endpoints;

public static class ItemTypeEndpoints
{
	public static void RegisterItemTypeAPIs(this IEndpointRouteBuilder app)
	{
		var itemTypeEndpoint = app.MapGroup("/item-type");

		itemTypeEndpoint.MapPost("/search", GetAllItemTypes).RequireAuthorization();
		itemTypeEndpoint.MapGet("/{id}", GetItemTypes).RequireAuthorization();
		itemTypeEndpoint.MapPost("/", CreateItemTypes).RequireAuthorization("Admin");
		itemTypeEndpoint.MapPut("/{id}", UpdateItemTypes).RequireAuthorization("Admin");
		itemTypeEndpoint.MapDelete("/{id}", DeleteItemTypes).RequireAuthorization("Admin");
	}
	private async static Task<IResult> GetAllItemTypes(CommonRequestHelper commonRequestHelper, IItemTypeService _service)
	{
		var result = await _service.GetAllItemTypesAsync(commonRequestHelper);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> GetItemTypes(int id, IItemTypeService _service)
	{
		var result = await _service.GetItemTypeByIdAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> CreateItemTypes(
		AddItemTypeRequestModel itemType,
		IItemTypeService _service,
		IValidator<AddItemTypeRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(itemType);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		var response = await _service.AddItemTypeAsync(itemType);
		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item Type has been created successfully!"));
	}

	private async static Task<IResult> UpdateItemTypes(
		int id,
		AddItemTypeRequestModel inputItemTypes,
		IItemTypeService _service,
		IValidator<AddItemTypeRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(inputItemTypes);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		var response = await _service.UpdateItemTypeAsync(id, inputItemTypes);
		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item Type has been updated successfully!"));
	}

	private async static Task<IResult> DeleteItemTypes(int id, IItemTypeService _service)
	{
		await _service.DeleteItemTypeAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(new(), "Item Type has been deleted successfully!"));
	}
}
