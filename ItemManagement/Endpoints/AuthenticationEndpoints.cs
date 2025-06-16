using FluentValidation;
using ItemManagement.Common.Helpers;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Endpoints;

public static class AuthenticationEndpoints
{
	public static void RegisterAuthenticationAPIs(this IEndpointRouteBuilder app)
	{
		var authenticationEndpoint = app.MapGroup("/auth");

		authenticationEndpoint.MapPost("/login", Login);
		authenticationEndpoint.MapPost("/forgot-password", ForgotPassword);
		authenticationEndpoint.MapPost("/reset-password", ResetPassword).RequireAuthorization();
	}

	private static async Task<IResult> Login(
		AuthenticateRequestModel user,
		IAuthenticationService authService
	)
	{
		var result = await authService.GenerateToken(user);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private static async Task<IResult> ForgotPassword(
		ForgotPasswordRequestModel forgotPassword,
		IAuthenticationService authService,
		IValidator<ForgotPasswordRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(forgotPassword);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}

		var result = await authService.ForgotPassword(forgotPassword);
		return Results.Ok(CommonResponseHelper.SuccessResponse(result));
	}

	private static async Task<IResult> ResetPassword(
		ResetPasswordRequestModel resetPassword,
		IAuthenticationService authService,
		IValidator<ResetPasswordRequestModel> validator
	)
	{
		var validationResult = await validator.ValidateAsync(resetPassword);
		if (!validationResult.IsValid)
		{
			return Results.ValidationProblem(validationResult.ToDictionary());
		}
		await authService.ResetPassword(resetPassword);
		return Results.Ok(CommonResponseHelper.SuccessResponse(new(), "Password has been changed successfully!"));
	}
}
