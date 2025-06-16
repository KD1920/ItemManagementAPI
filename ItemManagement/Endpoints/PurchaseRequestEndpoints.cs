using FluentValidation;
using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;
using ItemManagement.Common.Service.Interface;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Endpoints;

public static class PurchaseRequestEndpoints
{
	public static void RegisterPurchaseRequestAPIs(this IEndpointRouteBuilder app)
	{
		var purchaseRequestEndpoint = app.MapGroup("/purchase-request").RequireAuthorization("Admin");

		purchaseRequestEndpoint.MapPost("/search", GetAllPurchaseRequests);
		purchaseRequestEndpoint.MapGet("/{id}", GetPurchaseRequests);
		purchaseRequestEndpoint.MapPost("/", CreatePurchaseRequests);
	}
	private async static Task<IResult> GetAllPurchaseRequests(PurchaseRequestSearchParams searchParams, IPurchaseRequestService _service)
	{
		var result = await _service.GetAllPurchaseRequestsAsync(searchParams);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> GetPurchaseRequests(int id, IPurchaseRequestService _service)
	{
		var result = await _service.GetPurchaseRequestByIdAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> CreatePurchaseRequests(
		PurchaseRequestWithItemModels purchaseRequest,
		IValidator<PurchaseRequestWithItemModels> validator,
		IPurchaseRequestService _service,
		ICommonService commonService
	)
	{
		var user = await commonService.GetUser();
		purchaseRequest.ItemModels = commonService.CleanAndMergeItemModels(purchaseRequest.ItemModels);

		var validationResult = await validator.ValidateAsync(purchaseRequest);

		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var response = await _service.AddPurchaseRequestAsync(user, purchaseRequest);

		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "Purchase request created successfully!"));
	}
}
