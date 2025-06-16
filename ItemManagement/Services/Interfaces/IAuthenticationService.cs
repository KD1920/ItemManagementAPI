using ItemManagement.Data;
using System.Security.Claims;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Services.Interfaces;

public interface IAuthenticationService
{
	Task<string> GenerateToken(AuthenticateRequestModel user);
	ClaimsIdentity GenerateClaims(User user);
	Task<ForgotPasswordResponseModel> ForgotPassword(ForgotPasswordRequestModel forgotPassword);
	Task ResetPassword(ResetPasswordRequestModel resetPassword);
}
