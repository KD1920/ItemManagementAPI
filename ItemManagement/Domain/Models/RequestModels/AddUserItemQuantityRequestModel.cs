namespace ItemManagement.Domain.Models.RequestModels;

public class AddUserItemQuantityRequestModel
{
	public int ItemRequestId { get; set; }
	public int ItemModelId { get; set; }
	public int Quantity { get; set; }
}
