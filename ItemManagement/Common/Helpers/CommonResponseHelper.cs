namespace ItemManagement.Common.Helpers;

public static class CommonResponseHelper
{
	public static CommonApiResponseHelper SuccessResponse(object data, string? message = null)
	{
		return new()
		{
			StatusCode = System.Net.HttpStatusCode.OK,
			Data = data,
			IsSuccess = true,
			Message = !string.IsNullOrEmpty(message) ? message : ""
		};
	}
}
