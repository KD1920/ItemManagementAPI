using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class ItemReturnRequest
{
    public int Id { get; set; }

    public string RequestNumber { get; set; } = null!;

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<ItemReturnRequestItemModel> ItemReturnRequestItemModels { get; set; } = new List<ItemReturnRequestItemModel>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ReturnStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
