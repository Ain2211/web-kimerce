using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Setting;
using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Domain.WareHouse;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Infrastructure.Data;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.Services;
using Kimerce.Backend.Infrastructure.Services.Categories;
using Kimerce.Backend.Infrastructure.Services.EmailService;
using Kimerce.Backend.Infrastructure.Services.Images;
using Kimerce.Backend.Infrastructure.Services.Orders;
using Kimerce.Backend.Infrastructure.Services.Products;
using Kimerce.Backend.Infrastructure.Services.ShipmentItems;
using Kimerce.Backend.Infrastructure.Services.Shipments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using static System.Net.Mime.MediaTypeNames;
using Image = Kimerce.Backend.Domain.Images.Image;

using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Infrastructure.Services.Campaigns;
using Kimerce.Backend.Infrastructure.Services.Trackings;
using Kimerce.Backend.Infrastructure.Services.Trackings;
using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Domain.Tracking;
using Kimerce.Backend.Infrastructure.Services.Notifications;

using Kimerce.Backend.Infrastructure.Services.Campaigns;
using Kimerce.Backend.Infrastructure.Services.Attributes;
using Kimerce.Backend.Infrastructure.Services.AttributeValues;
using Kimerce.Backend.Infrastructure.Services.SpecAttributes;
using Kimerce.Backend.Infrastructure.Services.InventoryAttributes;
using Kimerce.Backend.Infrastructure.Services.ProductPieces;
using Kimerce.Backend.Infrastructure.Services.New;
using Kimerce.Backend.Infrastructure.Services.NewImage;

namespace Kimerce.Backend
{
    /// <summary>
    /// Startup
    /// </summary>

    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<KimDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            #endregion

            #region Services

            #region Setting
            services.AddScoped<ISettingService, SettingService>();
            #endregion

            #region Email
            services.AddScoped<IEmailProviderService, EmailProviderService>();
            services.AddScoped<IEmailAccountService, EmailAccountService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IEmailService, EmailService>();
            #endregion

            #region Shipments
            services.AddScoped<ShipmentService>();
            services.AddScoped<Repository<Shipment>>();
            services.AddScoped<ShipmentItemService>();
            services.AddScoped<Repository<ShipmentItem>>();
            #endregion
            #region Campaigns
            #region Campaign
            services.AddScoped<ICampaignService, CampaignService>();
            #endregion
            #region Discount
            services.AddScoped<IDiscountService, DiscountService>();
            #endregion
            #region CampaignOrder
            services.AddScoped<ICampaignOrderService, CampaignOrderService>();

            #endregion
            #region CampaignProduct
            services.AddScoped<ICampaignProductService, CampaignProductService>();
            #endregion
            #region DiscountOrder
            services.AddScoped<IDiscountOrderService, DiscountOrderService>();
            #endregion
            #endregion


            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IDistrictService, DistrictService>();

            services.AddScoped<IWardService, WardService>();
            #region Tracking
            services.AddScoped<ITrackingService, TrackingService>();
            services.AddScoped<Repository<Tracking>>();
            #endregion

            #region Warehouse
            services.AddScoped<IWareHouseService, WareHouseService>();
            #endregion

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();

            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<IWareHouseService, WareHouseService>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IImageService, ImageService>();
            #region News
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewsImageService, NewsImageService>();
            #endregion
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IRelateProductService, RelateProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IAttributeService, AttributeService>();
            services.AddScoped<IAttributeValueService, AttributeValueService>();
            services.AddScoped<ISpecAttributeService, SpecAttributeService>();
            services.AddScoped<IInventoryAttributeService, InventoryAttributeService>();
            services.AddScoped<IProductPieceService, ProductPieceService>();
            #endregion
            #region Notification
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<Repository<Notification>>();
            #endregion





            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });

                //c.IncludeXmlComments("/swagger/v1/swagger.json", true);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            app.UseSwagger();

            //app.UseReDoc(c =>
            //{
            //    c.SpecUrl = "/swagger/v1/swagger.json";
            //    c.DocumentTitle = "Product API V1";
            //    c.RoutePrefix = string.Empty;
            //});

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            AutoMapperConfig.Init();

        }
    }
}
