using AutoMapper;
using BookEcommerce.Models.DAL;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DAL.Repositories;
using BookEcommerce.Models.Entities;
using BookEcommerce.Models.Options;
using BookEcommerce.Services;
using BookEcommerce.Services.Interfaces;
using BookEcommerce.Services.Map;
using BookEcommerce.Services.Mapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("HuyGap", policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin();
    });
});
builder.Services.AddSession(options => {
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "payment";
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAutoMapper(options => options.AddProfile(typeof(MapperProfile)));
var localConnectionString = string.Empty;
localConnectionString = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(localConnectionString, b =>
    {
        b.MigrationsAssembly("BookEcommerce");
    });
    options.UseLazyLoadingProxies();
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
        RequireExpirationTime = true,
    };
});

//builder.Services.AddHttpLogging(logging =>
//{
//    logging.LoggingFields = HttpLoggingFields.All;
//    logging.RequestHeaders.Add("sec-ch-ua");
//    logging.ResponseHeaders.Add("MyResponseHeader");
//    logging.MediaTypeOptions.AddText("application/javascript");
//    logging.RequestBodyLogLimit = 4096;
//    logging.ResponseBodyLogLimit = 4096;
//});

//builder.Host.ConfigureLogging(logging =>
//{
//    logging.AddConsole();
//    logging.ClearProviders();
//});


//DB
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>()
//.AddScoped<DbFactory>();

services.AddScoped((Func<IServiceProvider, Func<ApplicationDbContext>>)((provider) => () => provider.GetService<ApplicationDbContext>()));
services.AddScoped<DbFactory>();
services.AddScoped(typeof(BookEcommerce.Models.DAL.Interfaces.IRepository<>), typeof(Repository<>));
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ILogger, Logger<PaymentService>>();
services.AddScoped<IMapperCustom,MapperCustom>();

builder.Services
    .AddScoped<MailSettings>()
    .AddScoped<MimeMessage>()
    .AddScoped<SmtpClient>();

//service
builder.Services
    .AddScoped<IAuthenticationService, AuthenticationService>()
    .AddScoped<IVerifyAccountService, VerifyAccountService>()
    .AddScoped<ITokenService, TokenService>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<IVendorService, VendorService>()
    .AddScoped<IAddressService, AddressService>()
    .AddScoped<IPaymentService, PaymentService>()
    .AddScoped<IPaypalContextService, PaypalContextService>();

services.AddScoped<ICartDetailService, CartDetailService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IProductService, ProductService>();
services.AddScoped<ICartService, CartService>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<ICartDetailService, CartDetailService>();
services.AddScoped<ISearchService, SearchService>();
//repo
builder.Services
    .AddScoped<IRoleRepository, RoleRepository>()
    //.AddScoped<Profile, MapperProfile>()
    .AddScoped<IMapper, AutoMapper.Mapper>()
    .AddScoped<ISendMailRepository, SendMailRepository>()
    .AddScoped<IAuthenticationRepository, AuthenticationRepository>()
    .AddScoped<ITokenRepository, TokenRepository>()
    .AddScoped<IVerifyAccountRepository, VerifyAccountRepository>()
    .AddScoped<IVendorRepository, VendorRepository>()
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>()
.AddScoped<AutoMapper.Profile, MapperProfile>()
//.AddScoped<IMapper, Mapper>()
.AddScoped<IAuthenticationRepository, AuthenticationRepository>()
.AddScoped<ITokenRepository, TokenRepository>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<ICartRepository, CartRepository>();
services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
services.AddScoped<IImageRepository, ImageRepository>();
services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
services.AddScoped<IPaymentRepository, PaymentRepository>();
services.AddScoped<ICartDetailRepository, CartDetailRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();

//configure
builder.Services.AddTransient<PaypalContext>();
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("HuyGap");
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
