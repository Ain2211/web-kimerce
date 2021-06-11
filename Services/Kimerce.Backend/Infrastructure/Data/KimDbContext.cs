using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Setting;
using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Domain.WareHouse;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Domain.News;
using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Infrastructure.Data.EntityMapping;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.Shipments;
using Microsoft.EntityFrameworkCore;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.Products;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.Campaigns;

using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.Notifications;
using Kimerce.Backend.Domain.Tracking;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.Trackings;
using Kimerce.Backend.Domain.Attributes;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.Attributes;
using Kimerce.Backend.Infrastructure.Data.EntityMapping.New;

namespace Kimerce.Backend.Infrastructure.Data
{
    public class KimDbContext : DbContext
    {
        public KimDbContext(DbContextOptions<KimDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder
                .RegisterEntityMapping<Setting, SettingMap>()
                .RegisterEntityMapping<EmailProvider, EmailProviderMap>()
                .RegisterEntityMapping<EmailAccount, EmailAccountMap>()
                .RegisterEntityMapping<EmailTemplate, EmailTemplateMap>()
                .RegisterEntityMapping<BaseCategory, BaseCategoryMap>()
                .RegisterEntityMapping<City, CityMap>()
                .RegisterEntityMapping<News, NewsMap>()
                .RegisterEntityMapping<NewsImage, NewsImageMap>()
                .RegisterEntityMapping<District, DistrictMap>()
                .RegisterEntityMapping<Ward, WardMap>()
                .RegisterEntityMapping<WareHouse, WareHouseMap>()
                .RegisterEntityMapping<Product, ProductMap>()
                .RegisterEntityMapping<RelateProduct, RelateProductMap>()
                .RegisterEntityMapping<ProductCategory, ProductCategoryMap>()

                .RegisterEntityMapping<Campaign, CampaignMap>()
                .RegisterEntityMapping<Discount, DiscountMap>()

                .RegisterEntityMapping<Notification, NotificationMap>()
                .RegisterEntityMapping<Tracking, TrackingMap>()
                .RegisterEntityMapping<Order, OrderMap>()

                .RegisterEntityMapping<CampaignOrder, CampaignOrderMap>()
                .RegisterEntityMapping<DiscountOrder, DiscountOrderMap>()
                .RegisterEntityMapping<CampaignProduct, CampaignProductMap>()


                .RegisterEntityMapping<Domain.Orders.Order, OrderMap>()
                .RegisterEntityMapping<Domain.Orders.Order_Item, OrderItemMap>()
                .RegisterEntityMapping<ProductImage, ProductImageMap>()
                
                .RegisterEntityMapping<Image, ImageMap>()


                .RegisterEntityMapping<Attribute,AttributeMap>()
                .RegisterEntityMapping<AttributeValue, AttributeValueMap>()
                .RegisterEntityMapping<SpecAttribute, SpecAttributeMap>()
                .RegisterEntityMapping<InventoryAttribute, InventoryAttributeMap>()
                .RegisterEntityMapping<ProductPiece, ProductPieceMap>()
              
                .RegisterEntityMapping<Shipment, ShipmentMap>()
                .RegisterEntityMapping<ShipmentItem, ShipmentItemMap>();
        }


    }
}