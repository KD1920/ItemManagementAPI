using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;

namespace ItemManagement.Endpoints;
public static class OpenEndpoints
{
    public static void RegisterOpenAPIs(this IEndpointRouteBuilder app)
    {
        var openEndpoint = app.MapGroup("/open");

        openEndpoint.MapGet("/item-type-options", GetAllItemTypeOptions);
        openEndpoint.MapGet("/item-model-options", GetAllItemModelOptions);
    }

    private async static Task<IResult> GetAllItemTypeOptions(IOpenService openService)
	{
		var result = await openService.GetAllItemTypeOptions();
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

    private async static Task<IResult> GetAllItemModelOptions(IOpenService openService)
	{
		var result = await openService.GetAllItemModelOptions();
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}
}