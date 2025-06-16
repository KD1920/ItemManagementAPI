using ItemManagement.Data;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Domain.Interface;

public interface IAuthenticationRepository
{
	Task<User> GetUserByEmailAsync(AuthenticateRequestModel authenticateRequestModel);
	Task<User> ForgotPassword(ForgotPasswordRequestModel forgotPassword);

	Task<User> ResetPassword(ResetPasswordRequestModel resetPassword);
}
