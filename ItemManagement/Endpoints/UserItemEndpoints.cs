using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Services.Interfaces;

namespace ItemManagement.Endpoints;

public static class UserItemEndpoints
{
	public static void RegisterUserItemAPIs(this IEndpointRouteBuilder app)
	{
		var itemRequestEndpoint = app.MapGroup("/user-item").RequireAuthorization();

		itemRequestEndpoint.MapPost("/", GetAllUserItemsAsync);
	}

	private async static Task<IResult> GetAllUserItemsAsync(UserItemSearchParams searchParams, IUserItemService _service)
	{
		var result = await _service.GetAllUserItemsAsync(searchParams);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}
}
