using FluentValidation;
using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Endpoints;

public static class UserEndpoints
{
	public static void RegisterUserAPIs(this IEndpointRouteBuilder app)
	{
		var userEndpoint = app.MapGroup("/user").RequireAuthorization("Admin");

		userEndpoint.MapPost("/search", GetAllUsers);
		userEndpoint.MapGet("/{id}", GetUsers);
		userEndpoint.MapPost("/", CreateUsers);
		userEndpoint.MapPut("/{id}", UpdateUsers);
		userEndpoint.MapDelete("/{id}", DeleteUsers);
	}
	private async static Task<IResult> GetAllUsers(UserSearchParams searchParams, IUserService _service)
	{
		var result = await _service.GetAllUsersAsync(searchParams);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> GetUsers(int id, IUserService _service)
	{
		var result = await _service.GetUserByIdAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private async static Task<IResult> CreateUsers(
		AddUserRequestModel user,
		IUserService _service,
		IValidator<AddUserRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(user);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		var response = await _service.AddUserAsync(user);
		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "User has been created successfully!"));
	}

	private async static Task<IResult> UpdateUsers(
		int id,
		AddUserRequestModel inputUsers,
		IUserService _service,
		IValidator<AddUserRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(inputUsers);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		var response = await _service.UpdateUserAsync(id, inputUsers);
		return Results.Ok(CommonResponseHelper.SuccessResponse(response, "User has been updated successfully!"));
	}

	private async static Task<IResult> DeleteUsers(int id, IUserService _service)
	{
		await _service.DeleteUserAsync(id);
		return Results.Ok(CommonResponseHelper.SuccessResponse(new(), "User has been deleted successfully!"));
	}
}
