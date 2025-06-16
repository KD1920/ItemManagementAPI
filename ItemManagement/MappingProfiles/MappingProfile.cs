using AutoMapper;
using ItemManagement.Common.Helpers;
using ItemManagement.Data;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.MappingProfile;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<User, AuthenticateRequestModel>();
		CreateMap<AuthenticateRequestModel, User>();

		CreateMap<User, GetUserResponseModel>();
		CreateMap<GetUserResponseModel, User>();

		CreateMap<User, GetUserWithPasswordResponseModel>();
		CreateMap<GetUserWithPasswordResponseModel, User>();

		CreateMap<User, AddUserRequestModel>();
		CreateMap<AddUserRequestModel, User>();

		CreateMap<GetUserResponseModel, AddUserRequestModel>();
		CreateMap<AddUserRequestModel, GetUserResponseModel>();

		CreateMap<ItemType, AddItemTypeRequestModel>();
		CreateMap<AddItemTypeRequestModel, ItemType>();

		CreateMap<ItemType, ItemTypeResponseModel>();
		CreateMap<ItemTypeResponseModel, ItemType>();

		CreateMap<ItemModel, AddItemModelRequestModel>();
		CreateMap<AddItemModelRequestModel, ItemModel>();

		CreateMap<ItemModel, ItemModelResponseModel>();
		CreateMap<ItemModelResponseModel, ItemModel>();

		CreateMap<PurchaseRequest, PurchaseRequestListResponseModel>();
		CreateMap<PurchaseRequestListResponseModel, PurchaseRequest>();

		CreateMap<PurchaseRequest, PurchaseRequestResponseModel>();
		CreateMap<PurchaseRequestResponseModel, PurchaseRequest>();

		CreateMap<PurchaseRequest, PurchaseRequestWithItemModels>();
		CreateMap<PurchaseRequestWithItemModels, PurchaseRequest>();

		CreateMap<PurchaseRequestItemResponseModel, PurchaseRequestItemModel>();
		CreateMap<PurchaseRequestItemModel, PurchaseRequestItemResponseModel>();

		CreateMap<PurchaseRequestItemResponseModel, PurchaseRequestItemModelRequestModel>();
		CreateMap<PurchaseRequestItemModelRequestModel, PurchaseRequestItemResponseModel>();

		CreateMap<PurchaseRequestItemModel, PurchaseRequestItemModelRequestModel>();
		CreateMap<PurchaseRequestItemModelRequestModel, PurchaseRequestItemModel>();

		CreateMap<ItemModel, PurchaseRequestItemModelRequestModel>();
		CreateMap<PurchaseRequestItemModelRequestModel, ItemModel>()
			.ForMember(
				dest => dest.Id,
				opt => opt.MapFrom(src => src.ItemModelId)
			);

		CreateMap<AddUserItemWithQuantityRequestModel, ItemRequest>();
		CreateMap<ItemRequest, AddUserItemWithQuantityRequestModel>();

		CreateMap<UserItemRequestResponseModel, ItemRequest>();
		CreateMap<ItemRequest, UserItemRequestResponseModel>();

		CreateMap<UpdateUserItemWithQuantityRequestModel, ItemRequest>();
		CreateMap<ItemRequest, UpdateUserItemWithQuantityRequestModel>();

		CreateMap<AddUserItemQuantityRequestModel, PurchaseRequestItemResponseModel>();
		CreateMap<PurchaseRequestItemResponseModel, AddUserItemQuantityRequestModel>();

		CreateMap<ItemRequestItemModel, AddUserItemQuantityRequestModel>();
		CreateMap<AddUserItemQuantityRequestModel, ItemRequestItemModel>();


		CreateMap<ItemRequestItemModel, PurchaseRequestItemResponseModel>();
		CreateMap<PurchaseRequestItemResponseModel, ItemRequestItemModel>();

		CreateMap<ItemModel, AddUserItemQuantityRequestModel>();
		CreateMap<AddUserItemQuantityRequestModel, ItemModel>()
		.ForMember(
				dest => dest.Id,
				opt => opt.MapFrom(src => src.ItemModelId)
			);


		CreateMap<ItemReturnRequest, AddUserItemWithQuantityRequestModel>();
		CreateMap<AddUserItemWithQuantityRequestModel, ItemReturnRequest>();


		CreateMap<ItemReturnRequest, UserItemRequestResponseModel>();
		CreateMap<UserItemRequestResponseModel, ItemReturnRequest>();

		CreateMap<ItemReturnRequestItemModel, AddUserItemQuantityRequestModel>();
		CreateMap<AddUserItemQuantityRequestModel, ItemReturnRequestItemModel>();

		CreateMap<ItemReturnRequestItemModel, UserItemRequestResponseModel>();
		CreateMap<UserItemRequestResponseModel, ItemReturnRequestItemModel>();

		CreateMap<ItemReturnRequestItemModel, PurchaseRequestItemResponseModel>();
		CreateMap<PurchaseRequestItemResponseModel, ItemReturnRequestItemModel>();

		CreateMap<UserItem, AddUserItemQuantityRequestModel>();
		CreateMap<AddUserItemQuantityRequestModel, UserItem>();

		CreateMap<UserItem, PurchaseRequestItemResponseModel>();
		CreateMap<PurchaseRequestItemResponseModel, UserItem>();

		CreateMap<UserItem, UserItemResponseModel>();
		CreateMap<UserItemResponseModel, UserItem>();

		CreateMap<CommonOptionResponseHelper, ItemType>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Option))
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Value))
			.ReverseMap()
			.ForMember(dest => dest.Option, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));

		CreateMap<CommonOptionResponseHelper, ItemModel>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Option))
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Value))
			.ReverseMap()
			.ForMember(dest => dest.Option, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));
	}
}
