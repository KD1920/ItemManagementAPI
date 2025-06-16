namespace ItemManagement.Domain.Models.ResponseModels;

public class UserItemResponseModel
{
    public int ItemModelId { get; set; }
    public int? Quantity { get; set; }
    public int UserId { get; set; }
}
