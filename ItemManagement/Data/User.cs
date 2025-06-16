using System;
using System.Collections.Generic;

namespace ItemManagement.Data;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public bool? IsAdmin { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseModifiedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<ItemModel> ItemModelCreatedByNavigations { get; set; } = new List<ItemModel>();

    public virtual ICollection<ItemModel> ItemModelModifiedByNavigations { get; set; } = new List<ItemModel>();

    public virtual ICollection<ItemRequest> ItemRequestCreatedByNavigations { get; set; } = new List<ItemRequest>();

    public virtual ICollection<ItemRequest> ItemRequestModifiedByNavigations { get; set; } = new List<ItemRequest>();

    public virtual ICollection<ItemRequest> ItemRequestUsers { get; set; } = new List<ItemRequest>();

    public virtual ICollection<ItemReturnRequest> ItemReturnRequestCreatedByNavigations { get; set; } = new List<ItemReturnRequest>();

    public virtual ICollection<ItemReturnRequest> ItemReturnRequestModifiedByNavigations { get; set; } = new List<ItemReturnRequest>();

    public virtual ICollection<ItemReturnRequest> ItemReturnRequestUsers { get; set; } = new List<ItemReturnRequest>();

    public virtual ICollection<ItemType> ItemTypeCreatedByNavigations { get; set; } = new List<ItemType>();

    public virtual ICollection<ItemType> ItemTypeModifiedByNavigations { get; set; } = new List<ItemType>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<PurchaseRequest> PurchaseRequestCreatedByNavigations { get; set; } = new List<PurchaseRequest>();

    public virtual ICollection<PurchaseRequestItemModel> PurchaseRequestItemModelCreatedByNavigations { get; set; } = new List<PurchaseRequestItemModel>();

    public virtual ICollection<PurchaseRequestItemModel> PurchaseRequestItemModelModifiedByNavigations { get; set; } = new List<PurchaseRequestItemModel>();

    public virtual ICollection<PurchaseRequest> PurchaseRequestModifiedByNavigations { get; set; } = new List<PurchaseRequest>();

    public virtual ICollection<PurchaseRequest> PurchaseRequestUsers { get; set; } = new List<PurchaseRequest>();

    public virtual ICollection<ReturnStatus> ReturnStatusCreatedByNavigations { get; set; } = new List<ReturnStatus>();

    public virtual ICollection<ReturnStatus> ReturnStatusModifiedByNavigations { get; set; } = new List<ReturnStatus>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Role> RoleCreatedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Role> RoleModifiedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Status> StatusCreatedByNavigations { get; set; } = new List<Status>();

    public virtual ICollection<Status> StatusModifiedByNavigations { get; set; } = new List<Status>();

    public virtual ICollection<UserItem> UserItemCreatedByNavigations { get; set; } = new List<UserItem>();

    public virtual ICollection<UserItem> UserItemModifiedByNavigations { get; set; } = new List<UserItem>();

    public virtual ICollection<UserItem> UserItemUsers { get; set; } = new List<UserItem>();
}
