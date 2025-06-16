using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class ItemReturnRequestItemModel
{
    public int Id { get; set; }

    public int ItemRequestId { get; set; }

    public int ItemModelId { get; set; }

    public int Quantity { get; set; }

    public virtual ItemModel ItemModel { get; set; } = null!;

    public virtual ItemReturnRequest ItemRequest { get; set; } = null!;
}
