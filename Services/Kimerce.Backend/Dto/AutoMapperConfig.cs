using AutoMapper;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Models.Images;
using Kimerce.Backend.Dto.Models.Orders;
using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Domain.Setting;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Domain.WareHouse;
using Kimerce.Backend.Domain.News;
using Kimerce.Backend.Dto.Categories;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Dto.Items.Categories.WareHouse;
using Kimerce.Backend.Dto.Items.Locations;
using Kimerce.Backend.Dto.Models.Categories;
using Kimerce.Backend.Dto.Items.Email;
using Kimerce.Backend.Dto.Items.Setting;
using Kimerce.Backend.Dto.Items.News;
using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Dto.Models.Shipments;
using System;
using Kimerce.Backend.Dto.Models.Attributes;
using Kimerce.Backend.Dto.Items.Attributes;
using Kimerce.Backend.Domain.Attributes;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Dto.Models.Campaigns;
using Kimerce.Backend.Dto.Items.Shipments;

using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Dto.Models.Notification;
using Kimerce.Backend.Domain.Tracking;
using Kimerce.Backend.Dto.Models.Tracking;

using Kimerce.Backend.Dto.Items.Campaigns;
using Kimerce.Backend.Dto.Items.Notification;
using Kimerce.Backend.Dto.Models.News;

namespace Kimerce.Backend.Dto
{
    public class AutoMapperConfig
    {
        #region Properties
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;

        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper => _mapper;

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration => _mapperConfiguration;
        #endregion

        public static void Init()
        {
            try
            {
                _mapperConfiguration = new MapperConfiguration(cfg =>
                {


                    #region Categories
                    #region City
                    cfg.CreateMap<City, City>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<City, CityModel>();
                    cfg.CreateMap<City, CityItem>();
                    cfg.CreateMap<CityModel, City>();
                    #endregion

                    #region District
                    cfg.CreateMap<District, District>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<District, DistrictModel>();
                    cfg.CreateMap<District, DistrictItem>();
                    cfg.CreateMap<DistrictModel, District>();
                    #endregion

                    #region Ward
                    cfg.CreateMap<Ward, Ward>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Ward, WardModel>();
                    cfg.CreateMap<Ward, WardItem>();
                    cfg.CreateMap<WardModel, Ward>();
                    #endregion

                    #region WareHouse
                    cfg.CreateMap<WareHouse, WareHouse>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<WareHouse, WareHouseModel>();
                    cfg.CreateMap<WareHouse, WareHouseItem>();
                    cfg.CreateMap<WareHouseModel, WareHouse>();
                    #endregion

                    #endregion

                    #region ProductCategory
                    cfg.CreateMap<ProductCategory, ProductCategory>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<ProductCategory, ProductCategoryItem>();
                    cfg.CreateMap<ProductCategory, ProductCategoryModel>();
                    cfg.CreateMap<ProductCategoryModel, ProductCategory>();
                    #endregion

                    #region Products
                    #region Product
                    cfg.CreateMap<Product, Product>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Product, ProductModel>();
                    cfg.CreateMap<Product, ProductItem>();
                    cfg.CreateMap<ProductModel, Product>();
                    #endregion
                    #region RelateProduct
                    cfg.CreateMap<RelateProduct, RelateProduct>()
                       .ForMember(dest => dest.Id, opt => opt.Ignore())
                       .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                       .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore());
                    cfg.CreateMap<RelateProduct, RelateProductItem>();
                    cfg.CreateMap<RelateProduct, RelateProductModel>();
                    cfg.CreateMap<RelateProductModel, RelateProduct>();
                    #endregion
                    #region ProductImage
                    cfg.CreateMap<ProductImage, ProductImage>()
                       .ForMember(dest => dest.Id, opt => opt.Ignore())
                       .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                       .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore());
                    cfg.CreateMap<ProductImage, ProductImageItem>();
                    cfg.CreateMap<ProductImage, ProductImageModel>();
                    cfg.CreateMap<ProductImageModel, ProductImage>();
                    #endregion
                    #endregion

                    #region Shipment
                    cfg.CreateMap<Shipment, Shipment>()
                        .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                        .ForMember(dest => dest.TrackingNumber, opt => opt.Ignore())
                        .ForMember(dest => dest.TotalWeight, opt => opt.Ignore())
                        .ForMember(dest => dest.DeliveryStatus, opt => opt.Ignore())
                        .ForMember(dest => dest.CompleteDate, opt => opt.Ignore())
                        .ForMember(dest => dest.HandoverDate, opt => opt.Ignore());
                    cfg.CreateMap<Shipment, ShipmentModel>();
                    cfg.CreateMap<ShipmentModel, Shipment>();
                    cfg.CreateMap<Shipment, Shipment_Item>();
                    #endregion
                    #region ShipmentItem
                    cfg.CreateMap<ShipmentItem, ShipmentItem>()
                        .ForMember(dest => dest.ShipmentId, opt => opt.Ignore())
                        .ForMember(dest => dest.OrderItemId, opt => opt.Ignore())
                        .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                        .ForMember(dest => dest.WareHouseId, opt => opt.Ignore());
                    cfg.CreateMap<ShipmentItem, ShipmentItemModel>();
                    cfg.CreateMap<ShipmentItemModel, ShipmentItem>();
                    cfg.CreateMap<ShipmentItem, ShipmentItem_Item>();
                    #endregion

                    #region Email
                    cfg.CreateMap<EmailProvider, EmailProvider>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<EmailProviderItem, EmailProvider>();
                    cfg.CreateMap<EmailProvider, EmailProviderItem>();

                    cfg.CreateMap<EmailAccount, EmailAccount>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<EmailAccountItem, EmailAccount>();
                    cfg.CreateMap<EmailAccount, EmailAccountItem>();

                    cfg.CreateMap<EmailTemplate, EmailTemplate>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<EmailTemplateItem, EmailTemplate>();
                    cfg.CreateMap<EmailTemplate, EmailTemplateItem>();

                    cfg.CreateMap<Email, Email>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<EmailItem, Email>();
                    cfg.CreateMap<Email, EmailItem>();
                    #endregion

                    #region Setting

                    cfg.CreateMap<Setting, Setting>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Setting, SettingItem>();
                    cfg.CreateMap<SettingItem, Setting>();

                    #endregion

                    #region Order

                    cfg.CreateMap<Order, Order>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Order, OrderModel>();
                    cfg.CreateMap<Order, OrderItem>();
                    cfg.CreateMap<OrderModel, Order>();

                    cfg.CreateMap<Domain.Orders.Order_Item, Domain.Orders.Order_Item>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Domain.Orders.Order_Item, OrderItemModel>();

                    cfg.CreateMap<Domain.Orders.Order_Item, Items.Orders.OrderItemItem>();
                    cfg.CreateMap<OrderItemModel, Domain.Orders.Order_Item>();
                    #endregion

                    #region Image

                    cfg.CreateMap<Image, Image>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        ;
                    cfg.CreateMap<Image, ImageModel>();
                    cfg.CreateMap<Image, ImageItem>();
                    cfg.CreateMap<ImageModel, Image>();
                    #endregion

                    #region News
                    #region New
                    cfg.CreateMap<News, News>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<News, NewsModel>();
                    cfg.CreateMap<News, NewsItem>();
                    cfg.CreateMap<NewsModel, News>();
                    #endregion
                    #region NewsImage
                    cfg.CreateMap<NewsImage, NewsImage>()
                       .ForMember(dest => dest.Id, opt => opt.Ignore())
                       .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                       .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore());
                    cfg.CreateMap<NewsImage, NewsImageItem>();
                    cfg.CreateMap<NewsImage, NewsImageModel>();
                    cfg.CreateMap<NewsImageModel, NewsImage>();
                    #endregion
                    #endregion
                    #region Campaigns
                    #region Campaign
                    cfg.CreateMap<Campaign, Campaign>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Campaign, CampaignModel>();
                    cfg.CreateMap<Campaign, CampaignItem>();
                    cfg.CreateMap<CampaignItem, Campaign>();
                    cfg.CreateMap<CampaignModel, Campaign>();
                    #endregion
                    #region Discount
                    cfg.CreateMap<Discount, Discount>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Discount, DiscountModel>();
                    cfg.CreateMap<Discount, DiscountItem>();
                    cfg.CreateMap<DiscountModel, Discount>();
                    #endregion
                    #region CampaignProduct
                    cfg.CreateMap<CampaignProduct, CampaignProduct>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<CampaignProduct, CampaignProductModel>();
                    cfg.CreateMap<CampaignProduct, CampaignProductItem>();
                    cfg.CreateMap<CampaignProductModel, CampaignProduct>();
                    #endregion
                    #region CampaignOrder
                    cfg.CreateMap<CampaignOrder, CampaignOrder>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<CampaignOrder, CampaignOrderModel>();
                    cfg.CreateMap<CampaignOrder, CampaignOrderItem>();
                    cfg.CreateMap<CampaignOrderModel, CampaignOrder>();
                    #endregion
                    #region DiscountOrder
                    cfg.CreateMap<DiscountOrder, DiscountOrder>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<DiscountOrder, DiscountOrderModel>();
                    cfg.CreateMap<DiscountOrder, DiscountOrderItem>();
                    cfg.CreateMap<DiscountOrderModel, DiscountOrder>();
                    #endregion
                    #endregion

                    #region Attributes
                    #region Attribute
                    cfg.CreateMap<Domain.Attributes.Attribute, Domain.Attributes.Attribute>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Domain.Attributes.Attribute, AttributeModel>();
                    cfg.CreateMap<Domain.Attributes.Attribute, AttributeItem>();
                    cfg.CreateMap<AttributeModel, Domain.Attributes.Attribute>();
                    #endregion
                    #region AttributeValue
                    cfg.CreateMap<AttributeValue, AttributeValue>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<AttributeValue, AttributeValueModel>();
                    cfg.CreateMap<AttributeValue, AttributeValueItem>();
                    cfg.CreateMap<AttributeValueModel, AttributeValue>();
                    #endregion
                    #region SpecAttribute
                    cfg.CreateMap<SpecAttribute, SpecAttribute>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<SpecAttribute, SpecAttributeModel>();
                    cfg.CreateMap<SpecAttribute, SpecAttributeItem>();
                    cfg.CreateMap<SpecAttributeModel, SpecAttribute>();
                    #endregion
                    #region InventoryAttribute
                    cfg.CreateMap<InventoryAttribute, InventoryAttribute>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<InventoryAttribute, InventoryAttributeModel>();
                    cfg.CreateMap<InventoryAttribute, InventoryAttributeItem>();
                    cfg.CreateMap<InventoryAttributeModel, InventoryAttribute>();
                    #endregion
                    #endregion

                    #region ProductPiece
                    cfg.CreateMap<ProductPiece, ProductPiece>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<ProductPiece, ProductPieceModel>();
                    cfg.CreateMap<ProductPiece, ProductPieceItem>();
                    cfg.CreateMap<ProductPieceModel, ProductPiece>();
                    #endregion

                    #region Notification
                    cfg.CreateMap<Notification, Notification>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Notification, NotificationModel>();
                    cfg.CreateMap<Notification, Notification>();

                    cfg.CreateMap<Notification_Order, Notification_Order>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Notification_Order, Notification_OrderModel>();
                    cfg.CreateMap<Notification_Order, Notification_OrderItem>();

                    cfg.CreateMap<Notification_OrderModel, Notification_Order>();

                    cfg.CreateMap<Notification_Image, Notification_Image>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Notification_Image, Notification_ImageModel>();
                    cfg.CreateMap<Notification_Image, Notification_ImageItem>();

                    cfg.CreateMap<Notification_ImageModel, Notification_Image>();

                    #endregion

                    #region Tracking
                    cfg.CreateMap<Tracking, Tracking>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
                        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
                    cfg.CreateMap<Tracking, TrackingModel>();
                    cfg.CreateMap<TrackingModel, Tracking>();

                    
                    #endregion
                });

                _mapper = _mapperConfiguration.CreateMapper();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}