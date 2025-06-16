using FluentValidation;
using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Endpoints;

public static class ItemModelEndpoints
{
	public static void RegisterItemModelAPIs(this IEndpointRouteBuilder app)
	{
		var itemModelEndpoint = app.MapGroup("/item-model");

		itemModelEndpoint.MapPost("/search", GetAllItemModels);
		// itemModelEndpoint.MapPost("/searchDemo", GetAllItemModelsDemo);
		// .RequireAuthorization();
		itemModelEndpoint.MapGet("/{id}", GetItemModels);
		// .RequireAuthorization();
		itemModelEndpoint.MapPost("/", CreateItemModels);
		// .RequireAuthorization("Admin");
		itemModelEndpoint.MapPut("/{id}", UpdateItemModels);
		// .RequireAuthorization("Admin");
		itemModelEndpoint.MapDelete("/{id}", DeleteItemModels);
		// .RequireAuthorization("Admin");
	}
	
	// private async static Task<IResult> GetAllItemModelsDemo()
	// {
	// 	//var result = await _service.GetAllItemModelsAsync(searchParams);
	// 	return Results.Ok(CommonResponseHelper.SuccessResponse("result"));
	// }
	private async static Task<IResult> GetAllItemModels(
		ItemModelSearchParams searchParams,
		IItemModelService _service
	)
	{
		var result = await _service.GetAllItemModelsAsync(searchParams);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> GetItemModels(int id, IItemModelService _service)
	{
		var result = await _service.GetItemModelByIdAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> CreateItemModels(
		AddItemModelRequestModel itemModel,
		IItemModelService _service,
		IValidator<AddItemModelRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(itemModel);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		var response = await _service.AddItemModelAsync(itemModel);
		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item Model has been created successfully!"));
	}

	private async static Task<IResult> UpdateItemModels(
		int id,
		AddItemModelRequestModel inputItemModels,
		IItemModelService _service,
		IValidator<AddItemModelRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(inputItemModels);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		var response = await _service.UpdateItemModelAsync(id, inputItemModels);
		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item Model has been updated successfully!"));
	}

	private async static Task<IResult> DeleteItemModels(int id, IItemModelService _service)
	{
		await _service.DeleteItemModelAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(new(), "Item Model has been deleted successfully!"));
	}
}
