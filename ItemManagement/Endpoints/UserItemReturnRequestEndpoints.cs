using FluentValidation;
using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;
using ItemManagement.Common.Service.Interface;
using ItemManagement.ApplicationConstants.Enums;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Endpoints;

public static class UserItemReturnRequestEndpoints
{
	public static void RegisterUserItemReturnRequestAPIs(this IEndpointRouteBuilder app)
	{
		var itemRequestEndpoint = app.MapGroup("/item-return-request").RequireAuthorization();

		itemRequestEndpoint.MapPost("/search", GetAllItemReturnRequestsAsync);
		itemRequestEndpoint.MapGet("/{id}", GetItemReturnRequestByIdAsync);
		itemRequestEndpoint.MapPost("/", AddItemReturnRequestAsync);
		itemRequestEndpoint.MapPost("/draft/", AddItemReturnRequestAsDraftAsync);
		itemRequestEndpoint.MapPut("/{id}", UpdateItemReturnRequestAsync);
	}
	private async static Task<IResult> GetAllItemReturnRequestsAsync(UserItemRequestSearchParams searchParams, IUserItemReturnRequestService _service)
	{
		var result = await _service.GetAllItemReturnRequestsAsync(searchParams);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> GetItemReturnRequestByIdAsync(int id, IUserItemReturnRequestService _service)
	{
		var result = await _service.GetItemReturnRequestByIdAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> AddItemReturnRequestAsync(
		AddUserItemWithQuantityRequestModel itemRequest,
		IUserItemReturnRequestService _service,
		IValidator<AddUserItemWithQuantityRequestModel> validator,
		ICommonService commonService
	)
	{
		var user = await commonService.GetUser();

		itemRequest.ItemModels = commonService.CleanAndMergeItemModels(itemRequest.ItemModels);

		var validationResult = await validator.ValidateAsync(itemRequest);

		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var response = await _service.AddItemReturnRequestAsync(itemRequest, (int)UserItemRequestStatus.Pending, user.UserId);

		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item return request created successfully!"));
	}

	private async static Task<IResult> UpdateItemReturnRequestAsync(
		int id,
		UpdateUserItemWithQuantityRequestModel itemRequest,
		IUserItemReturnRequestService _service,
		IValidator<UpdateUserItemWithQuantityRequestModel> validator,
		ICommonService commonService
	)
	{
		var user = await commonService.GetUser();

		itemRequest.ItemModels = commonService.CleanAndMergeItemModels(itemRequest.ItemModels);


		var validationResult = await validator.ValidateAsync(itemRequest);

		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var response = await _service.UpdateItemReturnRequestAsync(id, itemRequest, user.UserId);

		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item return request updated successfully!"));
	}

	private async static Task<IResult> AddItemReturnRequestAsDraftAsync(
		AddUserItemWithQuantityRequestModel itemRequest,
		IUserItemReturnRequestService _service,
		IValidator<AddUserItemWithQuantityRequestModel> validator,
		ICommonService commonService
	)
	{
		var validationResult = await validator.ValidateAsync(itemRequest);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		var user = await commonService.GetUser();
		var response = await _service.AddItemReturnRequestAsync(itemRequest, (int)UserItemRequestStatus.Draft, user.UserId);
		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item return request has been saved as a draft."));
	}
}