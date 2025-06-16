using FluentValidation;
using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;
using ItemManagement.Common.Service.Interface;
using ItemManagement.ApplicationConstants.Enums;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Endpoints;

public static class UserItemRequestEndpoints
{
	public static void RegisterUserItemRequestAPIs(this IEndpointRouteBuilder app)
	{
		var itemRequestEndpoint = app.MapGroup("/item-request").RequireAuthorization();

		itemRequestEndpoint.MapPost("/search", GetAllItemRequestsAsync);
		itemRequestEndpoint.MapGet("/{id}", GetItemRequestByIdAsync);
		itemRequestEndpoint.MapPost("/", AddItemRequestAsync);
		itemRequestEndpoint.MapPost("/draft/", AddItemRequestAsDraftAsync);
		itemRequestEndpoint.MapPut("/{id}", UpdateItemRequestAsync);
	}
	private async static Task<IResult> GetAllItemRequestsAsync(UserItemRequestSearchParams searchParams, IUserItemRequestService _service)
	{
		var result = await _service.GetAllItemRequestsAsync(searchParams);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> GetItemRequestByIdAsync(int id, IUserItemRequestService _service)
	{
		var result = await _service.GetItemRequestByIdAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> AddItemRequestAsync(
		AddUserItemWithQuantityRequestModel itemRequest,
		IUserItemRequestService _service,
		IValidator<AddUserItemWithQuantityRequestModel> validator,
		ICommonService commonService
	)
	{
		var user = await commonService.GetUser();

		var validationResult = await validator.ValidateAsync(itemRequest);

		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var response = await _service.AddItemRequestAsync(itemRequest, (int)UserItemRequestStatus.Pending, user.UserId);

		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item request created successfully!"));
	}

	private async static Task<IResult> UpdateItemRequestAsync(
		int id,
		UpdateUserItemWithQuantityRequestModel itemRequest,
		IUserItemRequestService _service,
		IValidator<UpdateUserItemWithQuantityRequestModel> validator,
		ICommonService commonService
	)
	{
		var validationResult = await validator.ValidateAsync(itemRequest);

		itemRequest.ItemModels = commonService.CleanAndMergeItemModels(itemRequest.ItemModels);

		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var response = await _service.UpdateItemRequestAsync(id, itemRequest);

		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item request updated successfully!"));
	}

	private async static Task<IResult> AddItemRequestAsDraftAsync(
		AddUserItemWithQuantityRequestModel itemRequest,
		IUserItemRequestService _service,
		IValidator<AddUserItemWithQuantityRequestModel> validator,
		ICommonService commonService
	)
	{
		var user = await commonService.GetUser();

		var validationResult = await validator.ValidateAsync(itemRequest);

		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var response = await _service.AddItemRequestAsync(itemRequest, (int)UserItemRequestStatus.Draft, user.UserId);

		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Item request has been saved as a draft."));
	}
}