using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Address;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CartService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CountryServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Customer;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerAddressSer;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.CustomerPointServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalRole;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.InternalUser;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.MainCategoryService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ManufactureService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.OrderStatusService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Product;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductDiscountServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductExportServices;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductImportService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.ProductIngredientService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Site;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.SubCategoryService;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.Unit;
using BetterHealthManagementAPI.BetterHealth2023.Business.Service.VNPay;
using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.AddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CountryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerAddressRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerPointRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.CustomerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.InternalUserAuthRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.MainCategoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ManufacturerRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderExecutionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderBatchRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderContactInfoRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderPickupRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderShipmentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderStatusRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.OrderHeaderRepos.OrderVNPayRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductDiscountRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductExportRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportBatchRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductImportRepos.ProductImportDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductDetailRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductImageRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientDescriptionRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductIngredientRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductParentRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.ProductRepos.ProductUserTargetRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.RoleRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteInventoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SiteRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.SubCategoryRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UnitRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UserWorkingSiteRepos;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("betterhealth-firebase.json")
            }));
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "betterhealth-firebase.json");
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BetterHealthManagementAPI", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.EnableAnnotations();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("JwtStorage:Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });
            JwtUserToken.Initialize(Configuration);
            EmailService.Initialize(Configuration);
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDbContext<BetterHealthManagementContext>();            
            services.AddCors(p => p.AddPolicy("MyCors", build =>
            {
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            //them tang service
            services.AddScoped<IInternalUserAuthService, InternalUserAuthService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMainCategoryService, MainCategoryService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IManufactureService, ManufactureService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IProductIngredientService, ProductIngredientService>();
            services.AddScoped<IProductImportService, ProductImportService>();
            services.AddTransient<IVNPayService, VNPayService>();
            services.AddScoped<IProductDiscountService, ProductDiscountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderStatusService, OrderStatusService>();
            services.AddScoped<ICustomerPointService, CustomerPointService>();
            services.AddScoped<IProductExportService, ProductExportService>();

            //them tang repo
            services.AddTransient<IInternalUserAuthRepo, InternalUserAuthRepo>();
            services.AddTransient<IDynamicAddressRepo, DynamicAddressRepo>();
            services.AddTransient<IUserWorkingSiteRepo, UserWorkingSiteRepo>();
            services.AddTransient<ISiteRepo, SiteRepo>();
            services.AddTransient<IOrderHeaderRepo, OrderHeaderRepo>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<IMainCategoryRepo, MainCategoryRepo>();
            services.AddTransient<ISubCategoryRepo, SubCategoryRepo>();
            services.AddTransient<IProductDescriptionRepo, ProductDescriptionRepo>();
            services.AddTransient<IProductDetailRepo, ProductDetailRepo>();
            services.AddTransient<IProductImageRepo, ProductImageRepo>();
            services.AddTransient<IProductIngredientDescriptionRepo, ProductIngredientDescriptionRepo>();
            services.AddTransient<IProductIngredientRepo, ProductIngredientRepo>();
            services.AddTransient<IProductParentRepo, ProductParentRepo>();
            services.AddTransient<ICustomerRepo, CustomerRepo>();
            services.AddTransient<ICustomerPointRepo, CustomerPointRepo>();
            services.AddTransient<ICustomerAddressRepo, CustomerAddressRepo>();
            services.AddTransient<IUnitRepo, UnitRepo>();
            services.AddTransient<IManufacturerRepo, ManufacturerRepo>();
            services.AddTransient<ICountryRepo, CountryRepo>();
            services.AddTransient<IProductImportRepo, ProductImportRepo>();
            services.AddTransient<IProductImportDetailRepo, ProductImportDetailRepo>();
            services.AddTransient<IProductImportBatchRepo, ProductImportBatchRepo>();
            services.AddTransient<ISiteInventoryRepo, SiteInventoryRepo>();
            services.AddTransient<IProductDiscountRepo, ProductDiscountRepo>();
            services.AddTransient<IProductEventDiscountRepo, ProductEventDiscountRepo>();
            services.AddTransient<IOrderShipmentRepo, OrderShipmentRepo>();
            services.AddTransient<IOrderPickUpRepo, OrderPickUpRepo>();
            services.AddTransient<IOrderBatchRepo, OrderBatchRepo>();
            services.AddTransient<IOrderDetailRepo, OrderDetailRepo>();
            services.AddTransient<IOrderContactInfoRepo, OrderContactInfoRepo>();
            services.AddTransient<IOrderStatusRepo, OrderStatusRepo>();
            services.AddTransient<IOrderExecutionRepo, OrderExecutionRepo>();
            services.AddTransient<IOrderVNPayRepo, OrderVNPayRepo>();
            services.AddTransient<IProductExportRepo, ProductExportRepo>();
            services.AddTransient<IProductUserTargetRepo, ProductUserTargetRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetterHealthManagementAPI v1"));
            } else
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BetterHealthManagementAPI v1"));
            }
            app.UseHttpsRedirection();

            app.UseCors("MyCors");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Map("/", context => Task.Run((() =>
                    context.Response.Redirect("/swagger/index.html"))));
            });
        }
    }
}
