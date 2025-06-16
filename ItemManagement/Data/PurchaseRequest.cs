using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class PurchaseRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? RequestDate { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public string? UserName { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<PurchaseRequestItemModel> PurchaseRequestItemModels { get; set; } = new List<PurchaseRequestItemModel>();

    public virtual User User { get; set; } = null!;
}
