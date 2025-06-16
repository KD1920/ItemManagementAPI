using System.ComponentModel;

namespace ItemManagement.ApplicationConstants.Enums;

public enum UserItemReturnStatus
{
	[Description("Draft")]
	Draft = 1,
	[Description("Pending")]
	Pending = 2,
	[Description("Approved")]
	Approved = 3,
	[Description("Cancelled")]
	Cancelled = 4,
	[Description("Rejected")]
	Rejected = 5
}
