using System.Text.Json;
using ItemManagement.Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ItemManagement.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await HandleException(context, ex);
		}
	}

	private static async Task HandleException(HttpContext context, Exception ex)
	{
		var (statusCode, message) = ex switch
		{
			UnauthorizedAccessException => (System.Net.HttpStatusCode.Unauthorized, "Unauthorized access."),
			KeyNotFoundException => (System.Net.HttpStatusCode.NotFound, "Data not found."),
			NotImplementedException => (System.Net.HttpStatusCode.NotImplemented, "This feature is not implemented."),
			ArgumentNullException => (System.Net.HttpStatusCode.BadRequest, "A required argument was null."),
			ArgumentException => (System.Net.HttpStatusCode.BadRequest, ex.Message),
			ValidationException validationEx => (System.Net.HttpStatusCode.BadRequest, validationEx.ValidationResult.ErrorMessage),
			ApplicationException => (System.Net.HttpStatusCode.BadRequest, ex.Message),
			JsonException => (System.Net.HttpStatusCode.BadRequest, "Invalid JSON format in request body."),
			_ when ex.InnerException is JsonException => (System.Net.HttpStatusCode.BadRequest, "Invalid JSON format in request body."),
			TimeoutException => (System.Net.HttpStatusCode.RequestTimeout, "The request timed out."),
			_ => (System.Net.HttpStatusCode.InternalServerError, "An unexpected error occurred.")
		};

		var errorResponse = new CommonApiResponseHelper
		{
			IsSuccess = false,
			StatusCode = statusCode
		};
		errorResponse.ErrorMessages.Add(message);

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)errorResponse.StatusCode;
		await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
	}
}