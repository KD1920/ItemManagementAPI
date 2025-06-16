using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class PurchaseRequestItemModel
{
    public int Id { get; set; }

    public int PurchaseRequestId { get; set; }

    public int ItemModelId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ItemModel ItemModel { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;
}
