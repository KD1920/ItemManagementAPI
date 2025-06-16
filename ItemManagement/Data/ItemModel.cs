using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class ItemModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int ItemTypeId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<ItemRequestItemModel> ItemRequestItemModels { get; set; } = new List<ItemRequestItemModel>();

    public virtual ICollection<ItemReturnRequestItemModel> ItemReturnRequestItemModels { get; set; } = new List<ItemReturnRequestItemModel>();

    public virtual ItemType ItemType { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<PurchaseRequestItemModel> PurchaseRequestItemModels { get; set; } = new List<PurchaseRequestItemModel>();

    public virtual ICollection<UserItem> UserItems { get; set; } = new List<UserItem>();
}
