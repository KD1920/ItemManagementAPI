using System.IdentityModel.Tokens.Jwt;
using ItemManagement.Common.Service.Interface;
using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Common.Service;

public class CommonService(IHttpContextAccessor httpContextAccessor) : ICommonService
{
	public async Task<CommonUserModel> GetUser()
	{
		var handler = new JwtSecurityTokenHandler();
		var tokenValue = httpContextAccessor?.HttpContext?.Request.Cookies["token"];
		var decodedToken = handler.ReadJwtToken(tokenValue);
		var user = new CommonUserModel();
		foreach (var claim in decodedToken.Claims)
		{
			switch (claim.Type)
			{
				case "userId":
					user.UserId = int.Parse(claim.Value);
					break;

				case "isAdmin":
					user.IsAdmin = claim.Value == "True";
					break;

				case "unique_name":
					user.Email = claim.Value.ToString();
					break;

				case "userName":
					user.Name = claim.Value.ToString();
					break;

				case "role":
					user.RoleName = claim.Value.ToString();
					break;

				case "roleId":
					user.RoleId = int.Parse(claim.Value);
					break;
			}
		}
		return await Task.FromResult(user);
	}
	public List<PurchaseRequestItemResponseModel> CleanAndMergeItemModels(List<PurchaseRequestItemResponseModel> items)
	{
		return items
			.Where(x => x.Quantity > 0)
			.GroupBy(x => x.ItemModelId)
			.Select(g => new PurchaseRequestItemResponseModel
			{
				ItemModelId = g.Key,
				Quantity = g.Sum(x => x.Quantity)
			})
			.ToList();
	}
}
