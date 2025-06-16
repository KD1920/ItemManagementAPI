using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<ItemRequest> ItemRequests { get; set; } = new List<ItemRequest>();

    public virtual User? ModifiedByNavigation { get; set; }
}
