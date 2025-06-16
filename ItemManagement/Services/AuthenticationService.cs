using System.Text;
using ItemManagement.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ItemManagement.Domain.Interface;
using ItemManagement.ApplicationConstants;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Services;

public class AuthenticationService(
	IAuthenticationRepository repository,
	IHttpContextAccessor httpContextAccessor,
	IGenericRepository<User> genericRepository
) : IAuthenticationService
{
	public async Task<string> GenerateToken(AuthenticateRequestModel user)
	{
		var userAlreadyExist = await genericRepository.ExistsAsync(x => x.Email == user.Email && x.Password == user.Password);
		if (!userAlreadyExist)
			throw new ArgumentException("Invalid username or password.");
		var handler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(Constants.PRIVATE_KEY);
		var credentials = new SigningCredentials(
			new SymmetricSecurityKey(key),
			SecurityAlgorithms.HmacSha256Signature
		);

		var userData = await repository.GetUserByEmailAsync(user);
		if (userData == null || (!userData.Active ?? false))
			throw new Exception("Invalid credentials or the user not found or user is inactive.");
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = GenerateClaims(userData),
			Expires = DateTime.UtcNow.AddDays(1),
			SigningCredentials = credentials,
		};

		var token = handler.CreateToken(tokenDescriptor);

		var tokenValue = handler.WriteToken(token);

		httpContextAccessor.HttpContext.Response.Cookies.Append(
			"token",
			tokenValue,
			new CookieOptions
			{
				Expires = DateTimeOffset.UtcNow.AddDays(1),
				HttpOnly = false,
				IsEssential = true,
				Secure = false,
				SameSite = SameSiteMode.Lax

			}
		);

		return tokenValue;
	}

	public ClaimsIdentity GenerateClaims(User user)
	{
		var claims = new ClaimsIdentity();
		claims.AddClaim(new Claim("userId", user.Id.ToString()));
		claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));
		claims.AddClaim(new Claim("userName", user.Name));
		claims.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name));
		claims.AddClaim(new Claim("roleId", user.Role.Id.ToString()));
		claims.AddClaim(new Claim("isAdmin", user.IsAdmin.ToString()));
		return claims;
	}

	public async Task<ForgotPasswordResponseModel> ForgotPassword(ForgotPasswordRequestModel forgotPassword)
	{
		var data = await repository.ForgotPassword(forgotPassword);
		var response = new ForgotPasswordResponseModel();
		response.Password = data.Password;
		return response;
	}

	public async Task ResetPassword(ResetPasswordRequestModel resetPassword)
	{
		await repository.ResetPassword(resetPassword);
	}
}
