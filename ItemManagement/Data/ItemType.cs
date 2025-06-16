using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class ItemType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<ItemModel> ItemModels { get; set; } = new List<ItemModel>();

    public virtual User? ModifiedByNavigation { get; set; }
}
