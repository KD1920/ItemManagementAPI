using ItemManagement.Data;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Domain.Interface;
using ItemManagement.Domain.Models.RequestModels;

namespace ItemManagement.Repository;

public class AuthenticationRepository(ItemManagementDbContext _context) : IAuthenticationRepository
{
	public async Task<User> GetUserByEmailAsync(AuthenticateRequestModel authenticateRequestModel)
	{
		var data = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(u => u.Email == authenticateRequestModel.Email && u.Password == authenticateRequestModel.Password);
		return data;
	}

	public async Task<User> ForgotPassword(ForgotPasswordRequestModel forgotPassword)
	{
		var data = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotPassword.Email);
		return data;
	}

	public async Task<User> ResetPassword(ResetPasswordRequestModel resetPassword)
	{
		var data = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPassword.Email && u.Password == resetPassword.Password);
		data.Password = resetPassword.NewPassword;
		_context.Users.Update(data);
		await _context.SaveChangesAsync();
		return data;
	}
}
